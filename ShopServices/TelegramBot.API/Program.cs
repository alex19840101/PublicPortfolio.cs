using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ServiceCollectionsExtensions;
using TelegramBot.API;
using TelegramBot.API.Interfaces;
using TelegramBot.API.Services;

const string SERVICE_NAME = "TelegramBot.API";
const string APPSETTINGS_BOT_SECTION = "TelegramBot";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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

builder.Services.AddScoped<ITelegramNotificationService, TelegramNotificationsService>();
builder.Services.AddScoped<TelegramUpdateHandler>();
builder.Services.AddScoped<TelegramUpdatesReceiverService>();
builder.Services.AddHostedService<PollingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("../swagger/v1/swagger.json", $"{SERVICE_NAME} API v1");
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
