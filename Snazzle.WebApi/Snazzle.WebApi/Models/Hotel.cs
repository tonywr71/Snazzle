using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snazzle.WebApi.Models
{
  public class Hotel
  {
    public int HotelId { get; set; }
    public string HotelName { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Address { get; set; }
    public int Order { get; set; }

    public ICollection<Room> Rooms { get; set; }
  }
}
