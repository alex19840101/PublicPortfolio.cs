using System;
using System.IO;
using System.Reflection;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NewsFeedSystem.API
{
    /// <summary>
    /// Класс настроек для генерации интерфейса и документации Swagger (OpenAPI)
    /// </summary>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private const string SERVICE_NAME = $"NewsFeedSystem";
        private const string DEVELOPER = "Shapovalov Alexey";
        private const string URL = "https://github.com/alex19840101/PublicPortfolio.cs/compare/MsCRUDs";

        /// <summary> Конструктор класса настроек для генерации интерфейса и документации Swagger (OpenAPI), инжектирующий провайдер версий API IApiVersionDescriptionProvider </summary>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) =>
            _provider = provider;


        /// <summary>
        /// Конфигурация интерфейса и документации Swagger (OpenAPI)
        /// </summary>
        public void Configure(SwaggerGenOptions options)
        {
            foreach(var description in _provider.ApiVersionDescriptions)
            {
                var apiVersion = description.ApiVersion.ToString();
                options.SwaggerDoc(description.GroupName,
                    new OpenApiInfo
                    {
                        Version = apiVersion,
                        Title = SERVICE_NAME,
                        Description = $"{SERVICE_NAME} Web API {apiVersion}",
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

                options.AddSecurityDefinition($"AuthToken {apiVersion}",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "bearer",
                        Name = "Authorization",
                        Description = "Authorization token"
                    });
                #region options.AddSecurityRequirement (добавить, если все методы требуют авторизации)---------------------------------------
                //options.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = $"AuthToken {apiVersion}"
                //            }
                //        },
                //        new string[] { }
                //    }
                //});
                #endregion options.AddSecurityRequirement (добавить, если все методы требуют авторизации)---------------------------------------
                //options.CustomOperationIds(apiDescription =>
                //    apiDescription.TryGetMethodInfo(out MethodInfo methodInfo)
                //        ? methodInfo.Name
                //        : null);
            }
        }
    }
}
