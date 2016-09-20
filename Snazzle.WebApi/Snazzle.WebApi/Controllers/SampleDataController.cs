using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Snazzle.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Snazzle.WebApi.Controllers
{
  [Route("api/[controller]")]
  public class SampleDataController : Controller
  {
    private ISampleDataService sampleDataService;

    public SampleDataController(ISampleDataService sampleDataService)
    {
      this.sampleDataService = sampleDataService;
    }

    [HttpGet("[action]")]
    [Authorize(ActiveAuthenticationSchemes="Bearer", Roles="Admins")]
    public IEnumerable<WeatherForecast> WeatherForecasts()
    {
      var identity = User.Identity as ClaimsIdentity;
      if (identity != null)
      {
        Debug.WriteLine(identity.Name);
      }

      return this.sampleDataService.GetAll();


    }

  }
}
