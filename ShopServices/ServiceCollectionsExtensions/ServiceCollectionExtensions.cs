using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Templates;
using Serilog.Templates.Themes;

namespace ServiceCollectionsExtensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Добавление Swagger и настроек версионирования API (OpenAPI)
        /// </summary>
        /// <param name="serviceCollection"> IServiceCollection-коллекция сервисов </param>
        /// <param name="serviceName"> название сервиса (API) </param>
        /// <param name="includeAbstractionsXml"> включать ли ShopServices.Abstractions.xml в Swagger(OpenAPI)-документацию </param>
        /// <returns> IServiceCollection-коллекция сервисов </returns>
        public static IServiceCollection AddSwaggerAndVersioning(
            this IServiceCollection serviceCollection,
            string serviceName,
            bool includeAbstractionsXml = true)
        {
            const string DEVELOPER = "Shapovalov Alexey";
            const string URL = "https://github.com/alex19840101/PublicPortfolio.cs/tree/main/ShopServices";
            serviceCollection.AddSwaggerGen(options => // Register the Swagger generator, defining 1 or more Swagger documents
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = serviceName,
                    Description = $"{serviceName} Web API v1",
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
                var xmlFile = $"{serviceName}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                if (includeAbstractionsXml)
                { 
                    xmlFile = "ShopServices.Abstractions.xml";
                    xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    options.IncludeXmlComments(xmlPath);
                    options.CustomSchemaIds(x => x.FullName);
                }
                //options.GeneratePolymorphicSchemas();
            });
            serviceCollection.AddApiVersioning(
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

            return serviceCollection;
        }

        /// <summary>
        /// Добавление Serilog-логирования
        /// </summary>
        /// <param name="serviceCollection"> IServiceCollection-коллекция сервисов </param>
        /// <param name="configuration"> IConfiguration-конфигурация ((ConfigurationManager)) </param>
        /// <returns> IServiceCollection-коллекция сервисов </returns>
        public static IServiceCollection AddSerilogging(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddSerilog((services, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.Console(new ExpressionTemplate(
                    template: "[{@t:HH:mm:ss} {@l:u3}{#if @tr is not null} ({substring(@tr,0,4)}:{substring(@sp,0,4)}){#end}] {@m}\n{@x}", // Include trace and span ids when present.
                    theme: TemplateTheme.Code)));

            return serviceCollection;
        }

        /// <summary>
        /// Добавление AuthorizationBuilder для JWT для аутентификации и авторизации по ролям
        /// </summary>
        /// <param name="serviceCollection"> IServiceCollection-коллекция сервисов</param>
        /// <returns> The <see cref="AuthorizationBuilder"/> so that additional calls can be chained. </returns>
        public static AuthorizationBuilder AddAuthorizationBuilderForJWT(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddAuthorizationBuilder()
                    .AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
                    {
                        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                        policy.RequireClaim(ClaimTypes.Role);
                    });
        }

        /// <summary>
        /// Добавление AuthenticationBuilder для JWT для аутентификации
        /// </summary>
        /// <param name="serviceCollection"> IServiceCollection-коллекция сервисов </param>
        /// <param name="tokenValidationParameters"> TokenValidationParameters-параметры валидации JWT-токена </param>
        /// <returns>A <see cref="AuthenticationBuilder"/> that can be used to further configure authentication.</returns>
        public static AuthenticationBuilder AddAuthenticationBuilderForJWT(this IServiceCollection serviceCollection, TokenValidationParameters tokenValidationParameters)
        {
            return serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = tokenValidationParameters;
                    options.IncludeErrorDetails = true;
                    options.SaveToken = true;
                });
        }

        /// <summary>
        /// Добавление Redis-кэширования
        /// </summary>
        /// <param name="serviceCollection"> IServiceCollection-коллекция сервисов </param>
        /// <param name="configuration"> IConfiguration-конфигурация ((ConfigurationManager)) </param>
        /// <returns> IServiceCollection-коллекция сервисов </returns>
        public static IServiceCollection AddStackExchangeRedisCaching(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            return serviceCollection.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = $"{configuration.GetValue<string>("Redis:Server")}:{configuration.GetValue<int>("Redis:Port")}";
            });
        }
    }
}
