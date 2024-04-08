using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.CookiePolicy;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.HttpOverrides;


using NLog.Web;
using NLog;
using Audit.WebApi;

using ztrm.Models;
using ztrm.Services.Audit;
using ztrm.Services.Interfaces;
using ztrm.Services;


var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    //First configure services for the webAppBuilder
    var builder = WebApplication.CreateBuilder(args);

    //Setup Nlog for dependency Injection.
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Warning);
    builder.Host.UseNLog();

    //Retrieve configuration settings
    builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    });


    //Set up DB Contexts
    //First we make the DBContext for the ZTRMContext.
    builder.Services.AddDbContext<ZTRMContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ZTRMContext")));

    //We also need to make the separate DBContext for the audit logs
    builder.Services.AddDbContext<AuditDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ZTRMContext")));

    //So Services config for cookies and Authentication and Authorization should go here if we want it.

    builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
    builder.Services.AddRazorPages();
    builder.Services.AddHttpContextAccessor();

    //Custom Services
    builder.Services.AddScoped<IRandomThoughtsService, RandomThoughtsService>();



    var app = builder.Build();

    //************************************* Configure App *******************************************************

    // Configure Audit.NET to use our custom data provider
    Audit.Core.Configuration.DataProvider = new SqlServerAuditDataProvider(app.Services.CreateScope().ServiceProvider.GetService<AuditDbContext>());

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    else
    {
        app.UseDeveloperExceptionPage();
    }

    IList<CultureInfo> sc = new List<CultureInfo>();
    sc.Add(new CultureInfo("en-US"));

    var lo = new RequestLocalizationOptions
    {
        DefaultRequestCulture = new RequestCulture("en-US"),
        SupportedCultures = sc,
        SupportedUICultures = sc
    };
    var cp = lo.RequestCultureProviders.OfType<CookieRequestCultureProvider>().First();

    app.UseRequestLocalization(lo);
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthorization();

    // Add Audit.NET middleware to audit all requests
    app.UseAuditMiddleware(config => config
     .FilterByRequest(req => !req.Path.StartsWithSegments("/health")) // Ignore health check requests
     .WithEventType("{verb}:{url}") // Custom event type including HTTP verb and URL
     .IncludeHeaders() // Include request and response headers
     .IncludeRequestBody() // Include the request body in the audit event
     .IncludeResponseBody() // Include the response body in the audit event
     .IncludeResponseHeaders() // Optionally include response headers
    );

    app.MapRazorPages();

    app.Run();
}
catch (Exception e)
{
    logger.Error(e, "Program stopped due to exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}