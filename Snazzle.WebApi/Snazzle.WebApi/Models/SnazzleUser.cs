using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using OpenIddict;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snazzle.WebApi.Models
{
  public class SnazzleUser : OpenIddictUser
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
  }
}
