using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Snazzle.WebApi.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Snazzle.WebApi.Controllers
{
  [Route("api/[controller]")]
  public class HotelDataController : Controller
  {
    private SnazzleDbContext snazzleDbContext;

    public HotelDataController(SnazzleDbContext snazzleDbContext)
    {
      this.snazzleDbContext = snazzleDbContext;
    }

    [HttpGet("[action]")]
    public IEnumerable<Hotel> GetHotels()
    {
      return this.snazzleDbContext.Hotels.Include(h=>h.Rooms);
    }
  }
}
