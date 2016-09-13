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

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Snazzle.WebApi.Controllers
{
  public class AuthorizationController : Controller
  {
    private OpenIddictUserManager<SnazzleUser> _userManager;

    public AuthorizationController(OpenIddictUserManager<SnazzleUser> userManager)
    {
      _userManager = userManager;
    }

    [HttpPost("~/connect/token")]
    [Produces("application/json")]
    public async Task<IActionResult> Exchange()
    {
      var request = HttpContext.GetOpenIdConnectRequest();

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
