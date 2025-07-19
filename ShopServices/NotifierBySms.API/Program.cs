using System;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using ServiceCollectionsExtensions;
using NotifierBySms.API;
using NotifierBySms.API.Interfaces;
using NotifierBySms.API.Services;
using NotifierBySms.API.Services.gRPC;
using Microsoft.Extensions.Logging;

const string SERVICE_NAME = "NotifierBySms.API";
const string APPSETTINGS_BOT_SECTION = "SmsBot";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IHostEnvironment env = builder.Environment;

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSwaggerAndVersioning(SERVICE_NAME);

builder.Services.Configure<SmsBotClientOptionsSettings>(config: builder.Configuration.GetSection(key: APPSETTINGS_BOT_SECTION));
var smsBotClientOptionsSettings = builder.Configuration.GetSection(key: APPSETTINGS_BOT_SECTION)?.Get<SmsBotClientOptionsSettings>();
builder.Services.AddHttpClient(name: "ShopServices.NotifierBySms.API.Client")
    .AddTypedClient<ISmsBotClient>(factory: (HttpClient httpClient, IServiceProvider serviceProvider) =>
    {
        var botSettings = serviceProvider.GetRequiredService<IOptions<SmsBotClientOptionsSettings>>().Value;
        ArgumentNullException.ThrowIfNull(botSettings);
        var botOptions = new SmsBotClientOptionsSettings(botSettings.BotToken);
        return new SmsBotClient(settings: botOptions, httpClient);
    });

builder.Services.AddGrpc();

builder.Services.AddHttpContextAccessor();

var tokenValidationParameters = builder.Configuration.GetTokenValidationParametersForJWT();

builder.Services.AddAuthenticationBuilderForJWT(tokenValidationParameters);

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
    {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireClaim(ClaimTypes.Role);
    });

builder.Services.AddScoped<IAuthorizationHandler, RoleAuthorizationHandler>();

builder.Services.AddSingleton<SmsBotClientOptionsSettings>(smsBotClientOptionsSettings!);
//builder.Services.AddScoped<ISmsNotificationsService, SmsNotificationsService>();
//builder.Services.AddScoped<ISmsNotificationsService, SmsNotificationsByAzureService>();


//var smsBotClientOptionsSettings = new SmsBotClientOptionsSettings(builder.Configuration[$"{APPSETTINGS_BOT_SECTION}:BotToken"])
//{
//    ConnectionString = builder.Configuration[$"{APPSETTINGS_BOT_SECTION}:ConnectionString"],
//};

builder.Services.AddScoped<ISmsNotificationsService>(src => new SmsNotificationsService(
    smsBotClientOptionsSettings: smsBotClientOptionsSettings,
    src.GetRequiredService<ILogger<SmsNotificationsService>>()));

builder.Services.AddScoped<ISmsNotificationsService>(src => new SmsNotificationsByAzureService(
    smsBotClientOptionsSettings: smsBotClientOptionsSettings,
    src.GetRequiredService<ILogger<SmsNotificationsByAzureService>>()));


//builder.Services.AddHostedService<SmsWorker>();

var isDevelopment = env.IsDevelopment();
if (isDevelopment)
{
    IdentityModelEventSource.ShowPII = true;
}

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

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(static endpoints =>
{
    //endpoints.MapGrpcService<GreeterService>(); //Debug
    endpoints.MapGrpcService<GrpcSmsNotificationsService>();
});

app.MapControllers();

app.Run();
