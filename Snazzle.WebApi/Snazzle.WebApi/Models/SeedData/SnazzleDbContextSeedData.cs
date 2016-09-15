using CryptoHelper;
using Microsoft.AspNetCore.Identity;
using OpenIddict;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Snazzle.WebApi.Models
{
  public class SnazzleDbContextSeedData
  {
    private SnazzleDbContext context;
    private UserManager<SnazzleUser> userManager;

    public SnazzleDbContextSeedData(SnazzleDbContext context, UserManager<SnazzleUser> userManager)
    {
      this.context = context;
      this.userManager = userManager;
    }

    public async Task EnsureSeedData()
    {
      context.Database.EnsureCreated();

      if (await this.userManager.FindByEmailAsync("tony@hotreb.com") == null)
      {
        var user = new SnazzleUser
        {
          UserName = "tony",
          Email = "tony@hotreb.com"
        };
        var result = await this.userManager.CreateAsync(user, "P@ssword1!");
        if (!result.Succeeded)
        {
          foreach (var error in result.Errors)
          {
            Debug.WriteLine(error.Description);
          }
        }
      }

      if (!context.Hotels.Any())
      {
        context.Hotels.Add(new Models.Hotel
        {
          HotelName = "Hilton",
          Address = "488 George St, Sydney NSW 2000",
          Latitude = -33.8719415,
          Longitude = 151.2058143,
          Order = 1,
          Rooms = new Room[] {
            new Room { RoomNumber="001" },
            new Room { RoomNumber="002" },
            new Room { RoomNumber="003" },
            new Room { RoomNumber="004" },
            new Room { RoomNumber="005" }
          }
        });
      }

      if (!context.Applications.Any())
      {
        context.Applications.Add(new OpenIddictApplication
        {
          // Note: these settings must match the application details
          // inserted in the database at the server level.
          ClientId = "snazzleClient",
          ClientSecret = Crypto.HashPassword("secret_secret_secret"),
          DisplayName = "Snazzle client application",
          LogoutRedirectUri = "http://localhost:5100/",
          RedirectUri = "http://localhost:5100/signin-oidc",
          Type = OpenIddictConstants.ClientTypes.Confidential
        });

        // To test this sample with Postman, use the following settings:
        // 
        // * Authorization URL: http://localhost:5100/connect/authorize
        // * Access token URL: http://localhost:5100/connect/token
        // * Client ID: postman
        // * Client secret: [blank] (not used with public clients)
        // * Scope: openid email profile roles
        // * Grant type: authorization code
        // * Request access token locally: yes
        context.Applications.Add(new OpenIddictApplication
        {
          ClientId = "postman",
          DisplayName = "Postman",
          RedirectUri = "https://www.getpostman.com/oauth2/callback",
          Type = OpenIddictConstants.ClientTypes.Public
        });
      }

      await this.context.SaveChangesAsync();


    }
  }
}
