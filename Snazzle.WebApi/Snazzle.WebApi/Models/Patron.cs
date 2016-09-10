using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snazzle.WebApi.Models
{
  public class Patron
  {
    public int PatronId { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
  }
}
