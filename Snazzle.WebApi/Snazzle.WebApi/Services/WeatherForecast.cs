﻿namespace Snazzle.WebApi.Services
{
  public class WeatherForecast
  {
    public string DateFormatted { get; set; }
    public int TemperatureC { get; set; }
    public string Summary { get; set; }

    public int TemperatureF
    {
      get
      {
        return 32 + (int)(this.TemperatureC / 0.5556);
      }
    }
  }
}