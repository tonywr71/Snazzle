using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snazzle.WebApi.Models;
using Snazzle.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snazzle.WebApi.Controllers
{
  [Route("api/[controller]")]
  public class PeopleDataController : Controller
  {
    private IPeopleDataService peopleDataService;

    public PeopleDataController(IPeopleDataService peopleDataService)
    {
      this.peopleDataService = peopleDataService;
    }

    [HttpGet("[action]")]
    public async Task<string> GetPeople()
    {
      return await this.peopleDataService.GetAll();
    }
  }
}
