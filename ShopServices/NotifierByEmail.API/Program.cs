using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using NotifierByEmail.API;
using NotifierByEmail.API.Interfaces;
using NotifierByEmail.API.Services;
using NotifierByEmail.API.Services.gRPC;
using ServiceCollectionsExtensions;

const string SERVICE_NAME = "NotifierByEmail.API";
const string APPSETTINGS_BOT_SECTION = "EmailBot";

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

var emailBotOptionsSettings = builder.Configuration.GetSection(key: APPSETTINGS_BOT_SECTION).Get<EmailBotOptionsSettings>();

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

builder.Services.AddScoped<IEmailNotificationsService>(src => new EmailNotificationsService(
    emailBotOptionsSettings: emailBotOptionsSettings!,
    src.GetRequiredService<ILogger<EmailNotificationsService>>()));


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
