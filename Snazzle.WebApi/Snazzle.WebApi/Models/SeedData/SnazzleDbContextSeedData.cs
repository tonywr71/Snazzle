using Microsoft.AspNetCore.Identity;
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
      if (await this.userManager.FindByEmailAsync("tony@hotreb.com")==null)
      {
        var user = new SnazzleUser
        {
          UserName = "tony",
          Email = "tony@hotreb.com"
        };
        var result = await this.userManager.CreateAsync(user, "P@ssword1!");
        if (!result.Succeeded)
        {
          foreach(var error in result.Errors)
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

      await this.context.SaveChangesAsync();


    }
  }
}
