using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenIddict;

namespace Snazzle.WebApi.Models
{
  public class SnazzleDbContext : OpenIddictDbContext<SnazzleUser>
  {
    private IConfigurationRoot config;

    public SnazzleDbContext(IConfigurationRoot config, DbContextOptions options) : base(options)
    {
      this.config = config;
    }

    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Patron> Patrons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      base.OnConfiguring(optionsBuilder);

      optionsBuilder.UseSqlServer(config["ConnectionStrings:SnazzleDbConnection"]);
    }
  }
}
