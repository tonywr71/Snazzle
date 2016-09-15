using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpenIddict;
using Snazzle.WebApi.Models;
using Microsoft.AspNetCore.Builder;
using AspNet.Security.OpenIdConnect.Extensions;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Authentication;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.AspNetCore.Identity;
using Snazzle.WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Snazzle.WebApi.Controllers
{
  public class AuthorizationController : Controller
  {
    private readonly OpenIddictApplicationManager<OpenIddictApplication> _applicationManager;
    private readonly SignInManager<SnazzleUser> _signInManager;
    private readonly OpenIddictUserManager<SnazzleUser> _userManager;

    public AuthorizationController(
        OpenIddictApplicationManager<OpenIddictApplication> applicationManager,
        SignInManager<SnazzleUser> signInManager,
        OpenIddictUserManager<SnazzleUser> userManager)
    {
      _applicationManager = applicationManager;
      _signInManager = signInManager;
      _userManager = userManager;
    }

    // Note: to support interactive flows like the code flow,
    // you must provide your own authorization endpoint action:

    [Authorize, HttpGet, Route("~/connect/authorize")]
    public async Task<IActionResult> Authorize(OpenIdConnectRequest request)
    {
      // Retrieve the application details from the database.
      var application = await _applicationManager.FindByClientIdAsync(request.ClientId);
      if (application == null)
      {
        return View("Error", new ErrorViewModel
        {
          Error = OpenIdConnectConstants.Errors.InvalidClient,
          ErrorDescription = "Details concerning the calling client application cannot be found in the database"
        });
      }

      // Flow the request_id to allow OpenIddict to restore
      // the original authorization request from the cache.
      return View(new AuthorizeViewModel
      {
        ApplicationName = application.DisplayName,
        RequestId = request.RequestId,
        Scope = request.Scope
      });
    }

    [Authorize, HttpPost("~/connect/authorize/accept"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Accept(OpenIdConnectRequest request)
    {
      // Retrieve the profile of the logged in user.
      var user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        return View("Error", new ErrorViewModel
        {
          Error = OpenIdConnectConstants.Errors.ServerError,
          ErrorDescription = "An internal error has occurred"
        });
      }

      // Create a new ClaimsIdentity containing the claims that
      // will be used to create an id_token, a token or a code.
      var identity = await _userManager.CreateIdentityAsync(user, request.GetScopes());

      // Create a new authentication ticket holding the user identity.
      var ticket = new AuthenticationTicket(
          new ClaimsPrincipal(identity),
          new AuthenticationProperties(),
          OpenIdConnectServerDefaults.AuthenticationScheme);

      ticket.SetResources(request.GetResources());
      ticket.SetScopes(request.GetScopes());

      // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
      return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
    }

    [Authorize, HttpPost("~/connect/authorize/deny"), ValidateAntiForgeryToken]
    public IActionResult Deny()
    {
      // Notify OpenIddict that the authorization grant has been denied by the resource owner
      // to redirect the user agent to the client application using the appropriate response_mode.
      return Forbid(OpenIdConnectServerDefaults.AuthenticationScheme);
    }

    [HttpGet("~/connect/logout")]
    public IActionResult Logout(OpenIdConnectRequest request)
    {
      // Flow the request_id to allow OpenIddict to restore
      // the original logout request from the distributed cache.
      return View(new LogoutViewModel
      {
        RequestId = request.RequestId
      });
    }

    [HttpPost("~/connect/logout"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
      // Ask ASP.NET Core Identity to delete the local and external cookies created
      // when the user agent is redirected from the external identity provider
      // after a successful authentication flow (e.g Google or Facebook).
      await _signInManager.SignOutAsync();

      // Returning a SignOutResult will ask OpenIddict to redirect the user agent
      // to the post_logout_redirect_uri specified by the client application.
      return SignOut(OpenIdConnectServerDefaults.AuthenticationScheme);
    }

    [HttpPost("~/connect/token")]
    [Produces("application/json")]
    public async Task<IActionResult> Exchange(OpenIdConnectRequest request)
    {
      if (request.IsPasswordGrantType())
      {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user == null)
        {
          return BadRequest(new OpenIdConnectResponse
          {
            Error = OpenIdConnectConstants.Errors.InvalidGrant,
            ErrorDescription = "The username/password couple is invalid."
          });
        }

        // Ensure the password is valid.
        if (!await _userManager.CheckPasswordAsync(user, request.Password))
        {
          if (_userManager.SupportsUserLockout)
          {
            await _userManager.AccessFailedAsync(user);
          }

          return BadRequest(new OpenIdConnectResponse
          {
            Error = OpenIdConnectConstants.Errors.InvalidGrant,
            ErrorDescription = "The username/password couple is invalid."
          });
        }

        if (_userManager.SupportsUserLockout)
        {
          await _userManager.ResetAccessFailedCountAsync(user);
        }

        var identity = await _userManager.CreateIdentityAsync(user, request.GetScopes());

        // Add a custom claim that will be persisted
        // in both the access and the identity tokens.
        identity.AddClaim("username", user.UserName,
            OpenIdConnectConstants.Destinations.AccessToken,
            OpenIdConnectConstants.Destinations.IdentityToken);

        // Create a new authentication ticket holding the user identity.
        var ticket = new AuthenticationTicket(
            new ClaimsPrincipal(identity),
            new AuthenticationProperties(),
            OpenIdConnectServerDefaults.AuthenticationScheme);

        ticket.SetResources(request.GetResources());
        ticket.SetScopes(request.GetScopes());

        return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
      }

      return BadRequest(new OpenIdConnectResponse
      {
        Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
        ErrorDescription = "The specified grant type is not supported."
      });
    }
  }
}
