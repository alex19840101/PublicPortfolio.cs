using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TelegramBot.API.Interfaces;

namespace TelegramBot.API.Services;

/// <summary> Сервис для опроса Telegram Bot API </summary>
public abstract class PollingServiceBase<TReceiverService> : BackgroundService
    where TReceiverService : ITelegramUpdatesReceiverService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PollingServiceBase<TReceiverService>> _logger;

    /// <summary> Конструктор сервиса для опроса Telegram Bot API </summary>
    public PollingServiceBase(
        IServiceProvider serviceProvider,
        ILogger<PollingServiceBase<TReceiverService>> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <summary> Задача для асинхронного выполнения (с возможностью отмены).
    /// <para>This method is called when the <see cref="IHostedService"/> starts. The implementation should return a task that represents the lifetime of the long running operation(s) being performed. </para></summary>
    /// <param name="cancellationToken">Triggered when <see cref="IHostedService.StopAsync(CancellationToken)"/> is called.</param>
    /// <returns>A <see cref="Task"/> that represents the long running operations.</returns>
    /// <remarks>See <see href="https://learn.microsoft.com/dotnet/core/extensions/workers">Worker Services in .NET</see> for implementation guidelines.</remarks>
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting polling service");

        await DoWork(cancellationToken);
    }

    private async Task DoWork(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var receiver = scope.ServiceProvider.GetRequiredService<TReceiverService>();

                await receiver.ReceiveUpdates(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("ReceiveUpdates exception: {Exception}", ex);

                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
        }
    }
}