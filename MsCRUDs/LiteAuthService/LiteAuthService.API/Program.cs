using System;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using LiteAuthService.API;
using LiteAuthService.BusinessLogic;
using LiteAuthService.Core.Repositories;
using LiteAuthService.Core.Services;
using LiteAuthService.DataAccess;
using LiteAuthService.DataAccess.Implementation;
using LiteAuthService.DataAccess.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Templates;
using Serilog.Templates.Themes;

const string SERVICE_NAME = $"MsCRUDs.LiteAuthService";
const string DEVELOPER = "Shapovalov Alexey";

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information($"Start service");
try
{
    var builder = WebApplication.CreateBuilder(args);

    IHostEnvironment env = builder.Environment;

    builder.Configuration
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
    string dataBaseConnectionStr = builder.Configuration.GetConnectionString("localdb");

    // Add services to the container.
    builder.Services.AddSerilog((services, loggerConfiguration) => loggerConfiguration
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console(new ExpressionTemplate(
                template: "[{@t:HH:mm:ss} {@l:u3}{#if @tr is not null} ({substring(@tr,0,4)}:{substring(@sp,0,4)}){#end}] {@m}\n{@x}", // Include trace and span ids when present.
                theme: TemplateTheme.Code)));

    builder.Services.AddControllers();

    builder.Services.AddAuthorizationBuilder()
        .AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
        {
            policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            policy.RequireClaim(ClaimTypes.Role);
        });

    builder.Services.AddHttpContextAccessor();

    builder.Services.AddScoped<IAuthorizationHandler, RoleAuthorizationHandler>();
    var tokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(builder.Configuration["JWT:KEY"])),
        ValidateIssuerSigningKey = true
    };

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = tokenValidationParameters;
            options.IncludeErrorDetails = true;
            options.SaveToken = true;
        });

    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    builder.Services.AddScoped<IDapperAsyncExecutor>(src => new DapperSqlExecutor(dataBaseConnectionStr));

    builder.Services.AddScoped<IAuthRepository, AuthRepository>();

    builder.Services.AddScoped<IAuthService>(src => new AuthService(
        src.GetRequiredService<IAuthRepository>(),
                    tokenValidationParameters,
                    key: builder.Configuration["JWT:KEY"]));

    var isDevelopment = env.IsDevelopment();

    #region -------------------------------Swagger-------------------------------
    const string URL = "https://github.com/alex19840101/PublicPortfolio.cs/compare/MsCRUDs";
    builder.Services.AddSwaggerGen(c => // Register the Swagger generator, defining 1 or more Swagger documents
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = SERVICE_NAME,
            Description = $"{SERVICE_NAME} Web API v1",
            TermsOfService = new Uri(URL),
            Contact = new OpenApiContact
            {
                Name = DEVELOPER,
                Email = string.Empty,
                Url = new Uri(URL),
            },
            License = new OpenApiLicense
            {
                Name = DEVELOPER,
                Url = new Uri(URL),
            }
        });
        //c.OperationFilter<SwaggerCustomFilters.AuthHeaderFilter>();

        // Set the comments path for the Swagger JSON and UI.
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
        xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.Contracts.xml";
        xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
        c.CustomSchemaIds(x => x.FullName);
        c.GeneratePolymorphicSchemas();
    });
    #endregion -------------------------------Swagger-------------------------------


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