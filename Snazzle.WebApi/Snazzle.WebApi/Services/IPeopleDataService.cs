using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snazzle.WebApi.Services
{
  public interface IPeopleDataService
  {
    Task<string> GetAll();
  }
}
