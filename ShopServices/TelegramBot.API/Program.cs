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
using TelegramBot.API;
using TelegramBot.API.Interfaces;
using TelegramBot.API.Services;
using TelegramBot.API.Services.gRPC;

const string SERVICE_NAME = "TelegramBot.API";
const string APPSETTINGS_BOT_SECTION = "TelegramBot";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IHostEnvironment env = builder.Environment;

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSwaggerAndVersioning(SERVICE_NAME, includeAbstractionsXml: false);

builder.Services.Configure<TelegramBotClientOptionsSettings>(config: builder.Configuration.GetSection(key: APPSETTINGS_BOT_SECTION));

builder.Services.AddHttpClient(name: "ShopServices.Telegram.Bot.Client")
    .AddTypedClient<Telegram.Bot.ITelegramBotClient>(factory: (HttpClient httpClient, IServiceProvider serviceProvider) =>
    {
        var botSettings = serviceProvider.GetRequiredService<IOptions<TelegramBotClientOptionsSettings>>().Value;
        ArgumentNullException.ThrowIfNull(botSettings);
        ArgumentNullException.ThrowIfNull(botSettings.BotToken);
        var botOptions = new Telegram.Bot.TelegramBotClientOptions(botSettings.BotToken);
        return new Telegram.Bot.TelegramBotClient(options: botOptions, httpClient);

    });

builder.Services.AddGrpc();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
    {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireClaim(ClaimTypes.Role);
    });

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAuthorizationHandler, RoleAuthorizationHandler>();
var tokenValidationParameters = builder.Configuration.GetTokenValidationParametersForJWT();
//var tokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//{
//    ValidateIssuer = true,
//    ValidIssuer = builder.Configuration["JWT:Issuer"],
//    ValidateAudience = true,
//    ValidAudience = builder.Configuration["JWT:Audience"],
//    ValidateLifetime = true,
//    IssuerSigningKey = new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(builder.Configuration["JWT:KEY"]!)),
//    ValidateIssuerSigningKey = true
//};

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = tokenValidationParameters;
//        options.IncludeErrorDetails = true;
//        options.SaveToken = true;
//    });
builder.Services.AddAuthenticationBuilderForJWT(tokenValidationParameters);


builder.Services.AddScoped<ITelegramNotificationService, TelegramNotificationsService>();
builder.Services.AddScoped<TelegramUpdateHandler>();
builder.Services.AddScoped<TelegramUpdatesReceiverService>();
builder.Services.AddHostedService<PollingService>();

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
    endpoints.MapGrpcService<GrpcTgNotificationsService>();
});

app.MapControllers();

app.Run();
