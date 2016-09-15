using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Snazzle.WebApi.Services;
using Snazzle.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.IO;
using OpenIddict;
using CryptoHelper;

namespace Snazzle.WebApi
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
      services.AddSingleton(Configuration);

      // Add framework services.
      services.AddCors();
      services.AddMvc();

      services.AddDbContext<SnazzleDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:SnazzleDbConnection"]));

      services.AddScoped<IPeopleDataService, PeopleDataService>();

      services.AddScoped<ISampleDataService, RealDataService>();

      services.AddTransient<SnazzleDbContextSeedData>();

      services.AddIdentity<SnazzleUser, IdentityRole>(config =>
      {
        config.User.RequireUniqueEmail = true;
        config.Password.RequiredLength = 8;
      }).AddEntityFrameworkStores<SnazzleDbContext>()
      .AddDefaultTokenProviders();

      services.AddOpenIddict<SnazzleUser, SnazzleDbContext>()

        // Register the ASP.NET Core MVC binder used by OpenIddict.
        // Note: if you don't call this method, you won't be able to
        // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
        .AddMvcBinders()

        // Enable the authorization, logout, and userinfo endpoints.
        .EnableAuthorizationEndpoint("/connect/authorize")
        .EnableLogoutEndpoint("/connect/logout")
        .EnableUserinfoEndpoint("/connect/userinfo")

        // Enable the token endpoint (required to use the password flow).
        .EnableTokenEndpoint("/connect/token")

        // Allow client applications to use the grant_type=password flow.
        .AllowPasswordFlow()

        // Note: the Mvc.Client sample only uses the code flow and the password flow, but you
        // can enable the other flows if you need to support implicit or client credentials.
        .AllowAuthorizationCodeFlow()
        .AllowRefreshTokenFlow()

        // During development, you can disable the HTTPS requirement.
        .DisableHttpsRequirement()

        // Register a new ephemeral key, that is discarded when the application
        // shuts down. Tokens signed using this key are automatically invalidated.
        // This method should only be used during development.
        .AddEphemeralSigningKey();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, SnazzleDbContextSeedData seeder)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      app.UseCors(builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });

      app.UseIdentity();

      app.UseOAuthValidation();

      app.UseOpenIddict();

      app.UseMvc();

      using (var context = new SnazzleDbContext(
                this.Configuration,
                app.ApplicationServices.GetRequiredService<DbContextOptions<SnazzleDbContext>>()))
      {
        context.Database.EnsureCreated();

        if (!context.Applications.Any())
        {
          context.Applications.Add(new OpenIddictApplication
          {
            // Note: these settings must match the application details
            // inserted in the database at the server level.
            ClientId = "snazzleClient",
            ClientSecret = Crypto.HashPassword("secret_secret_secret"),
            DisplayName = "Snazzle client application",
            LogoutRedirectUri = "http://localhost:5100/",
            RedirectUri = "http://localhost:5100/signin-oidc",
            Type = OpenIddictConstants.ClientTypes.Confidential
          });

          // To test this sample with Postman, use the following settings:
          // 
          // * Authorization URL: http://localhost:5100/connect/authorize
          // * Access token URL: http://localhost:5100/connect/token
          // * Client ID: postman
          // * Client secret: [blank] (not used with public clients)
          // * Scope: openid email profile roles
          // * Grant type: authorization code
          // * Request access token locally: yes
          context.Applications.Add(new OpenIddictApplication
          {
            ClientId = "postman",
            DisplayName = "Postman",
            RedirectUri = "https://www.getpostman.com/oauth2/callback",
            Type = OpenIddictConstants.ClientTypes.Public
          });

          context.SaveChanges();
        }
      }

      seeder.EnsureSeedData().Wait();

    }
  }
}
