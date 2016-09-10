using Shouldly;
using Snazzle.Test.MockService;
using Snazzle.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Snazzle.Test
{
  public class PeopleDataTest
  {

    [Fact(DisplayName = "There should be two genders")]
    public async void ThereShouldBeTwoGenders()
    {
      var controller = new PeopleDataController(new FakePeopleDataService());
      var people = await controller.GetPeople();
      (people.Length).ShouldBeGreaterThan(0);
    }


  }
}
