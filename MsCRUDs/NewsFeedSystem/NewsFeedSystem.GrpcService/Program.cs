using System;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using NewsFeedSystem.BusinessLogic;
using NewsFeedSystem.Core.Repositories;
using NewsFeedSystem.Core.Services;
using NewsFeedSystem.DataAccess;
using NewsFeedSystem.DataAccess.Repositories;
using Serilog;
using Serilog.Templates;
using Serilog.Templates.Themes;
using NewsFeedSystem.GrpcService.Services;
using NewsFeedSystem.GrpcService;

const string SERVICE_NAME = $"NewsFeedSystem";

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

    // Add services to the container.
    builder.Services.AddSerilog((services, loggerConfiguration) => loggerConfiguration
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console(new ExpressionTemplate(
                template: "[{@t:HH:mm:ss} {@l:u3}{#if @tr is not null} ({substring(@tr,0,4)}:{substring(@sp,0,4)}){#end}] {@m}\n{@x}", // Include trace and span ids when present.
                theme: TemplateTheme.Code)));

    builder.Services.AddGrpc();

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
    
    builder.Services.AddScoped<IAuthRepository, AuthRepository>();
    builder.Services.AddScoped<IAuthService>(src => new AuthService(
        src.GetRequiredService<IAuthRepository>(),
                    tokenValidationParameters,
                    key: builder.Configuration["JWT:KEY"]!));

    builder.Services.AddSingleton<ICacheService, CacheService>();

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = $"{builder.Configuration.GetValue<string>("Redis:Server")}:{builder.Configuration.GetValue<int>("Redis:Port")}";
    });

    builder.Services.AddScoped<INewsRepository, NewsRepository>();
    builder.Services.AddScoped<INewsService, NewsService>();

    builder.Services.AddScoped<ITagsRepository, TagsRepository>();
    builder.Services.AddScoped<ITagsService, TagsService>();

    builder.Services.AddScoped<ITopicsRepository, TopicsRepository>();
    builder.Services.AddScoped<ITopicsService, TopicsService>();

    string dataBaseConnectionStr = builder.Configuration.GetConnectionString("localdb")!;

    var isDevelopment = env.IsDevelopment();

    if (isDevelopment)
    {
        IdentityModelEventSource.ShowPII = true;
        builder.Services.AddDbContext<NewsFeedSystemDbContext>(builder =>
        {
            builder.UseSqlServer(connectionString: dataBaseConnectionStr)
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging();

            builder.LogTo(Console.WriteLine);
        });
    }
    else
    {
        builder.Services.AddDbContext<NewsFeedSystemDbContext>(builder =>
        {
            builder.UseSqlServer(connectionString: dataBaseConnectionStr);
        });
    }

    builder.Services.AddDefaultIdentity<IdentityUser>(
        options => options.SignIn.RequireConfirmedAccount = true)
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<NewsFeedSystemDbContext>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (isDevelopment)
    {
        app.UseDeveloperExceptionPage();
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

    // Configure the HTTP request pipeline.
    //app.MapGrpcService<GreeterService>();
    //app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(static endpoints =>
    {
        endpoints.MapGrpcService<GreeterService>();
        endpoints.MapGrpcService<GrpcAuthService>();
        endpoints.MapGrpcService<GrpcNewsService>();
        endpoints.MapGrpcService<GrpcTagsService>();
        endpoints.MapGrpcService<GrpcTopicsService>();
    });

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