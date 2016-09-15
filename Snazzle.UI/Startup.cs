using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using NWebsec.AspNetCore.Middleware;

namespace Snazzle
{
  public class Startup
  {
    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
          .AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      // Add framework services.
      //services.AddMvc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      app.UseCsp(options => options
               .DefaultSources(directive => directive.Self())
               //.ConnectSources(directive=>directive.CustomSources("http://localhost:5100/api/SampleData/WeatherForecasts"))
               .ConnectSources(directive=>directive.CustomSources("*") )
               .ImageSources(directive => directive.Self()
                   .CustomSources("*"))
               .ScriptSources(directive => directive.Self()
               .UnsafeEval()
                   .UnsafeInline())
               .FontSources(directive=>directive.Self()
                   .CustomSources("data:"))
               .StyleSources(directive => directive.Self()
                   .UnsafeInline()));

      app.UseXContentTypeOptions();

      app.UseXfo(options => options.Deny());

      app.UseXXssProtection(options => options.EnabledWithBlockMode());

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
        {
          HotModuleReplacement = true
        });
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
      }

      //The following code enables angular 2 deep linking
      app.Use(async (context, next) =>
      {
        await next();

        if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
        {
          context.Request.Path = "/"; // Angular root page here 
          await next();
        }

      });

      app.UseDefaultFiles();
      app.UseStaticFiles();

      //app.UseMvc(routes =>
      //{
      //    routes.MapRoute(
      //        name: "default",
      //        template: "{controller=Home}/{action=Index}/{id?}");

      //    routes.MapSpaFallbackRoute(
      //        name: "spa-fallback",
      //        defaults: new { controller = "Home", action = "Index" });
      //});
    }
  }
}
