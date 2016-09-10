using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snazzle.WebApi.Services
{
  public interface ISampleDataService
  {
    IEnumerable<WeatherForecast> GetAll();
  }
}
