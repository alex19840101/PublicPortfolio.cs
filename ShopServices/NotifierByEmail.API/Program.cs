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
using NotifierByEmail.API;
using NotifierByEmail.API.Interfaces;
using NotifierByEmail.API.Services;
using NotifierByEmail.API.Services.gRPC;
using Microsoft.Extensions.Logging;

const string SERVICE_NAME = "NotifierByEmail.API";
const string APPSETTINGS_BOT_SECTION = "EmailBot";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IHostEnvironment env = builder.Environment;

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSwaggerAndVersioning(SERVICE_NAME);

builder.Services.Configure<EmailBotOptionsSettings>(config: builder.Configuration.GetSection(key: APPSETTINGS_BOT_SECTION));

//builder.Services.AddHttpClient(name: "ShopServices.NotifierByEmail.API.Client")
//    .AddTypedClient<IEmailBotClient>(factory: (HttpClient httpClient, IServiceProvider serviceProvider) =>
//    {
//        var botSettings = serviceProvider.GetRequiredService<IOptions<EmailBotOptionsSettings>>().Value;
//        ArgumentNullException.ThrowIfNull(botSettings);
//        var botOptions = new EmailBotOptionsSettings(botSettings.BotToken);
//        return new EmailBotClient(options: botOptions, httpClient);
//    });

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

builder.Services.AddSingleton<EmailBotOptionsSettings>();
//builder.Services.AddScoped<IEmailNotificationsService, EmailNotificationsService>();
var emailBotOptionsSettings = new EmailBotOptionsSettings
{
    BotToken = builder.Configuration[$"{APPSETTINGS_BOT_SECTION}:BotToken"]
};


builder.Services.AddScoped<IEmailNotificationsService>(src => new EmailNotificationsService(
    emailBotOptionsSettings: emailBotOptionsSettings,
    src.GetRequiredService<ILogger<EmailNotificationsService>>()));
//builder.Services.AddHostedService<EmailWorker>();

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
    endpoints.MapGrpcService<GrpcEmailNotificationsService>();
});

app.MapControllers();

app.Run();
