// See https://aka.ms/new-console-template for more information
using System;
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
using NotificationsSender;
using Serilog;
using ServiceCollectionsExtensions;
using ShopServices.BusinessLogic;
using ShopServices.Core.Auth;
using ShopServices.Core.Repositories;
using ShopServices.Core.Services;
using ShopServices.DataAccess;
using ShopServices.DataAccess.Repositories;
using TelegramBot.API.Services.gRPC.Notifications;

const string SERVICE_NAME = "NotificationsSender";

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
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

    builder.Services.AddSerilogging(builder.Configuration);

    builder.Services.AddControllers();

    builder.Services.AddAuthorizationBuilderForJWT();

    builder.Services.AddHttpContextAccessor();

    builder.Services.AddScoped<IAuthorizationHandler, RoleAuthorizationHandler>();
    var tokenValidationParameters = builder.Configuration.GetTokenValidationParametersForJWT();

    builder.Services.AddAuthenticationBuilderForJWT(tokenValidationParameters);

    builder.Services.AddOpenApi();

    string dataBaseConnectionStr = builder.Configuration.GetConnectionString("ShopServices")!;
    string defaultDataBaseConnectionStr = builder.Configuration.GetConnectionString("DefaultConnection")!;

    var isDevelopment = env.IsDevelopment();

    builder.Services.AddDbContext<ShopServicesDbContext>(builder =>
    {
        builder.UseNpgsql(connectionString: defaultDataBaseConnectionStr, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();

        builder.LogTo(Console.WriteLine);
        //builder.ConfigureWarnings(wcb => wcb.Ignore(RelationalEventId.PendingModelChangesWarning));
    });


    if (isDevelopment)
    {
        IdentityModelEventSource.ShowPII = true;
        builder.Services.AddDbContext<ShopServicesDbContext>(builder =>
        {
            builder.UseNpgsql(connectionString: dataBaseConnectionStr, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging();

            builder.LogTo(Console.WriteLine);
            //builder.ConfigureWarnings(wcb => wcb.Ignore(RelationalEventId.PendingModelChangesWarning));
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

    builder.Services.AddSwaggerAndVersioning(SERVICE_NAME, includeAbstractionsXml: false);

    builder.Services.AddGrpcClient<GrpcTgNotifications.GrpcTgNotificationsClient>(options =>
    {
        options.Address = new Uri("https://localhost:7248");
    });
    var jwtSettings = builder.Configuration.GetSection("JWT").Get<JwtSettings>();
    
    builder.Services.AddScoped<IEmailNotificationsRepository, EmailNotificationsRepository>();
    builder.Services.AddScoped<IPhoneNotificationsRepository, PhoneNotificationsRepository>();
    builder.Services.AddScoped<INotificationsSenderService>(src => new NotificationsSenderService(
        src.GetRequiredService<IEmailNotificationsRepository>(),
                   src.GetRequiredService<IPhoneNotificationsRepository>(),
                   tokenValidationParameters,
                    key: builder.Configuration["JWT:KEY"]!));
    
    builder.Services.AddHostedService<NotificationWorker>();

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


/*

Console.WriteLine("NotificationsSender started");

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        var jwtSettings = configuration.GetSection("JWT").Get<JwtSettings>();

        services.AddGrpcClient<GrpcTgNotifications.GrpcTgNotificationsClient>(options =>
        {
            options.Address = new Uri("https://localhost:7248");
        });

        services.AddHostedService<NotificationWorker>();
    })
    .Build();

host.Run();
*/


/*
using var channel = GrpcChannel.ForAddress("https://localhost:7248");
var grpcTgNotificationClient = new TelegramBot.API.Services.gRPC.Notifications.GrpcTgNotifications.GrpcTgNotificationsClient(channel);

var configurations = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddUserSecrets<JwtSettings>().Build();

Console.WriteLine($"\n{configurations["JWT:KEY"]}");


var builder = WebApplication.CreateBuilder(args);
IHostEnvironment env = builder.Environment;
#if DEBUG
env.EnvironmentName = "Development";
#endif

builder.Configuration
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));
var jwtSettings = builder.Configuration.GetSection("JWT").Get<JwtSettings>();
//var jwtSettings = builder.Configuration.Get<JwtSettings>();

var tokenValidationParameters = builder.Configuration.GetTokenValidationParametersForJWT();

var claims = new List<Claim>
            {
                //new Claim(ClaimTypes.Name, loginData.Login),
                new Claim(ClaimTypes.Role, Roles.NotificationsSender)
            };

var jwt = new JwtSecurityToken(
    issuer: tokenValidationParameters.ValidIssuer,
    audience: tokenValidationParameters.ValidAudience,
    claims: claims,
    expires: DateTime.UtcNow.Add(TimeSpan.FromHours(24)),
    signingCredentials: new SigningCredentials(
        key: new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(builder.Configuration["JWT:KEY"]!)),
        algorithm: SecurityAlgorithms.HmacSha256));

var token = new JwtSecurityTokenHandler().WriteToken(jwt);

var headers = RequestHeadersPreparator.GetMetadataWithAuthorizationHeader(token);
var sendTgNotificationRequest = new SendTgNotificationRequest
{
    ChatId = testChatId,
    Message = "Test notification to Telegram"
};
await grpcTgNotificationClient.SendNotificationAsync(sendTgNotificationRequest, headers);
Console.WriteLine($"\nPress any key to exit");

Console.ReadLine();
*/
