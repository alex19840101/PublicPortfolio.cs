using System;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Asp.Versioning;
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
using Microsoft.OpenApi.Models;
using NewsFeedSystem.API;
using NewsFeedSystem.BusinessLogic;
using NewsFeedSystem.Core.Repositories;
using NewsFeedSystem.Core.Services;
using NewsFeedSystem.DataAccess;
using NewsFeedSystem.DataAccess.Repositories;
using Serilog;
using Serilog.Templates;
using Serilog.Templates.Themes;

const string DEVELOPER = "Shapovalov Alexey";
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
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
        .AddUserSecrets<Program>();

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
    builder.Services.AddOpenApi();

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
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
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

    #region -------------------------------Swagger-------------------------------
    const string URL = "https://github.com/alex19840101/PublicPortfolio.cs/compare/MsCRUDs";
    builder.Services.AddSwaggerGen(options => // Register the Swagger generator, defining 1 or more Swagger documents
    {
        options.SwaggerDoc("v1", new OpenApiInfo
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

        options.AddSecurityDefinition($"AuthToken v1",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "bearer",
                        Name = "Authorization",
                        Description = "Authorization token"
                    });


        //options.OperationFilter<SwaggerCustomFilters.AuthHeaderFilter>();

        // Set the comments path for the Swagger JSON and UI.
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath);
        xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.Contracts.xml";
        xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath);
        options.CustomSchemaIds(x => x.FullName);
        options.GeneratePolymorphicSchemas();
    });
    builder.Services.AddApiVersioning(
                        options =>
                        {
                            // reporting api versions will return the headers
                            // "api-supported-versions" and "api-deprecated-versions"
                            options.ReportApiVersions = true;

                            options.Policies.Sunset(0.9)
                                            .Effective(DateTimeOffset.Now.AddDays(60))
                                            .Link("policy.html")
                                                .Title("Versioning Policy")
                                                .Type("text/html");
                        })
                    .AddMvc()
                    .AddApiExplorer(
                        options =>
                        {
                            // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                            // note: the specified format code will format the version as "'v'major[.minor][-status]"
                            options.GroupNameFormat = "'v'VVV";

                            // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                            // can also be used to control the format of the API version in route templates
                            options.SubstituteApiVersionInUrl = true;
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