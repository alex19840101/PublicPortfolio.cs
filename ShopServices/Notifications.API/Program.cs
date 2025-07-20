using System;
using Notifications.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Serilog;
using ServiceCollectionsExtensions;
using ShopServices.BusinessLogic;
using ShopServices.Core.Repositories;
using ShopServices.Core.Services;
using ShopServices.DataAccess;
using ShopServices.DataAccess.Repositories;

const string SERVICE_NAME = "Notifications.API";

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information($"Start service");
try
{
    var builder = WebApplication.CreateBuilder(args);

    IHostEnvironment env = builder.Environment;

    builder.Configuration
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
        .AddUserSecrets<Program>();

    builder.Services.AddSerilogging(builder.Configuration);

    builder.Services.AddControllers();

    builder.Services.AddAuthorizationBuilderForJWT();

    builder.Services.AddHttpContextAccessor();

    builder.Services.AddScoped<IAuthorizationHandler, RoleAuthorizationHandler>();
    var tokenValidationParameters = builder.Configuration.GetTokenValidationParametersForJWT();

    builder.Services.AddAuthenticationBuilderForJWT(tokenValidationParameters);

    builder.Services.AddOpenApi();

    builder.Services.AddScoped<IEmailNotificationsRepository, EmailNotificationsRepository>();
    builder.Services.AddScoped<IPhoneNotificationsRepository, PhoneNotificationsRepository>();
    builder.Services.AddScoped<INotificationsService>(src => new NotificationsService(
        src.GetRequiredService<IEmailNotificationsRepository>(),
                   src.GetRequiredService<IPhoneNotificationsRepository>()));

    string dataBaseConnectionStr = builder.Configuration.GetConnectionString("ShopServices")!;

    var isDevelopment = env.IsDevelopment();

    if (isDevelopment)
    {
        IdentityModelEventSource.ShowPII = true;
        builder.Services.AddDbContext<ShopServicesDbContext>(builder =>
        {
            builder.UseNpgsql(connectionString: dataBaseConnectionStr, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging();

            builder.LogTo(Console.WriteLine);
            builder.ConfigureWarnings(wcb => wcb.Ignore(RelationalEventId.PendingModelChangesWarning));
        });
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    }
    else
    {
        builder.Services.AddDbContext<ShopServicesDbContext>(builder =>
        {
            builder.UseNpgsql(connectionString: dataBaseConnectionStr, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        });
    }

    builder.Services.AddDefaultIdentity<IdentityUser>(
        options => options.SignIn.RequireConfirmedAccount = true)
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ShopServicesDbContext>();

    builder.Services.AddSwaggerAndVersioning(SERVICE_NAME);
   
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (isDevelopment)
    {
        app.UseDeveloperExceptionPage();
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("../swagger/v1/swagger.json", $"{SERVICE_NAME} API v1");
        });
    }
    else
    {
        app.UseExceptionHandler(new ExceptionHandlerOptions()
        {
            AllowStatusCode404Response = true,
            ExceptionHandlingPath = "/error"
        });
        app.UseHsts();
    }

    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapGet("/", () => "Service is working");
    await app.RunAsync();

    Log.Information("Service stopped");
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occurred during bootstrapping");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}