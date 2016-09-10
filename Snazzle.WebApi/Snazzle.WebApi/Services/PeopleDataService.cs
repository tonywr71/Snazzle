using Snazzle.WebApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Snazzle.WebApi.Services
{
  public class PeopleDataService : IPeopleDataService
  {
    public static readonly string peopleJsonUrl = "http://agl-developer-test.azurewebsites.net/people.json";
    public async Task<string> GetAll()
    {
      var memoryStream = new MemoryStream();
      var req = WebRequest.CreateHttp(peopleJsonUrl);
      using (WebResponse response = await req.GetResponseAsync())
      {
        using (Stream responseStream = response.GetResponseStream())
        {
          // Read the bytes in responseStream and copy them to content. 
          await responseStream.CopyToAsync(memoryStream);
        }
      }
      memoryStream.Position = 0;
      using (var streamReader = new StreamReader(memoryStream))
      {
        var jsonString = await streamReader.ReadToEndAsync();
        return jsonString;
      }

    }
  }
}
