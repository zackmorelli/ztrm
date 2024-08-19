using Microsoft.EntityFrameworkCore;
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
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
    builder.Host.UseNLog();

    // This method pulls in configurations set in 'appsettings.json'
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        // Configuration is automatically bound to Kestrel
    });

    //Retrieve configuration settings
    builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    });


    //Set up DB Contexts
    //First we make the DBContext for the ZTRMContext.
    builder.Services.AddDbContext<ZTRMContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("ZTRMContext")));

    //We also need to make the separate DBContext for the audit logs
    builder.Services.AddDbContext<AuditDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("ZTRMContext")));

    //So Services config for cookies and Authentication and Authorization should go here if we want it.

    builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
    builder.Services.AddRazorPages();
    builder.Services.AddHttpContextAccessor();

    //Custom Services
    builder.Services.AddScoped<IRandomThoughtsService, RandomThoughtsService>();



    var app = builder.Build();

    //************************************* Configure App *******************************************************

    app.UseMiddleware<ztrm.Middleware.ErrorLogger>();

    // Configure Audit.NET to use our custom data provider
    Audit.Core.Configuration.DataProvider = new GenericAuditDataProvider(app.Services.CreateScope().ServiceProvider.GetService<AuditDbContext>());

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

    List<CultureInfo> cultureInfoList = new List<CultureInfo>();
    cultureInfoList.Add(new CultureInfo("en-US"));

    RequestLocalizationOptions localizationOptions = new RequestLocalizationOptions
    {
        DefaultRequestCulture = new RequestCulture("en-US"),
        SupportedCultures = cultureInfoList,
        SupportedUICultures = cultureInfoList
    };
    CookieRequestCultureProvider cultureProvider = localizationOptions.RequestCultureProviders.OfType<CookieRequestCultureProvider>().First();

    app.UseRequestLocalization(localizationOptions);
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthorization();

    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });


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