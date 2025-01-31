using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ProjectTasksTrackService.BusinessLogic;
using ProjectTasksTrackService.Core.Repositories;
using ProjectTasksTrackService.Core.Services;
using ProjectTasksTrackService.DataAccess;
using ProjectTasksTrackService.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IProjectsRepository, ProjectsRepository>();
builder.Services.AddScoped<IProjectsService, ProjectsService>();

builder.Services.AddScoped<ISubProjectsRepository, SubProjectsRepository>();
builder.Services.AddScoped<ISubProjectsService, SubProjectsService>();

builder.Services.AddScoped<ITasksRepository, TasksRepository>();
builder.Services.AddScoped<ITasksService, TasksService>();

IHostEnvironment env = builder.Environment;

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
string dataBaseConnectionStr = builder.Configuration.GetConnectionString("ProjectTasksTrackServiceDb");

var isDevelopment = env.IsDevelopment();

if (isDevelopment)
{
    builder.Services.AddDbContext<ProjectTasksTrackServiceDbContext>(builder =>
    {
        builder.UseNpgsql(connectionString: dataBaseConnectionStr, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();

        builder.LogTo(Console.WriteLine);
    });
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}
else
{
    builder.Services.AddDbContext<ProjectTasksTrackServiceDbContext>(builder =>
    {
        builder.UseNpgsql(connectionString: dataBaseConnectionStr, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
    });
}

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
if (isDevelopment)
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("../swagger/v2/swagger.json", "ProjectTasksTrackService API v2");
    });
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Service is working");
app.Run();
