using System;
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

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NLog.Web;
using NLog;

using ztrm.Models;
using Microsoft.AspNetCore.HttpOverrides;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    //First configure services for the webAppBuilder
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    });

    // Add services to the IOC container.
    builder.Services.AddDbContext<ZTRMContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ZTRMContext")));

    //So Services config for cookies and Authentication and Authorization should go here if we want it.


    builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
    builder.Services.AddRazorPages();
    builder.Services.AddHttpContextAccessor();

    //Setup Nlog for dependency Injection. this needs to go last
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();


    //Custom Services



    var app = builder.Build();

    //************************************* Configure App *******************************************************

    //handles HTTP request forwarded from Apache reverse proxy server
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });

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