// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("SimpleTradeWatcher started\nStart consuming process...");

var builder = Host.CreateApplicationBuilder();

builder.Services.AddHostedService<LegacyTradeEventConsumerJob>();

builder.Build().Run();
