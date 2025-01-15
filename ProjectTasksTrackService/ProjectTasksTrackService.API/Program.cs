using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ProjectTasksTrackService.BusinessLogic;
using ProjectTasksTrackService.Core.Repositories;
using ProjectTasksTrackService.Core.Services;
using ProjectTasksTrackService.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IProjectsRepository, ProjectsRepository>();
builder.Services.AddScoped<IProjectsService, ProjectsService>();

#region -------------------------------Swagger-------------------------------
const string URL = "https://github.com/alex19840101/PublicPortfolio.cs/compare/ProjectTasksTrackService";
builder.Services.AddSwaggerGen(c => // Register the Swagger generator, defining 1 or more Swagger documents
{
    c.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "ProjectTasksTrackService",
        Description = "ProjectTasksTrackService Web API v2",
        TermsOfService = new Uri(URL),
        Contact = new OpenApiContact
        {
            Name = "Shapovalov Alexey",
            Email = string.Empty,
            Url = new Uri(URL),
        },
        License = new OpenApiLicense
        {
            Name = "Shapovalov Alexey",
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
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("../swagger/v2/swagger.json", "ProjectTasksTrackService API v2");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Service is working");
app.Run();
