using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using TelegramBot.API.Interfaces;

namespace TelegramBot.API.Services;

/// <summary> Получатель обновлений из Telegram </summary>
public class TelegramUpdatesReceiverServiceBase<TUpdateHandler> : ITelegramUpdatesReceiverService
    where TUpdateHandler : Telegram.Bot.Polling.IUpdateHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly Telegram.Bot.Polling.IUpdateHandler _updateHandler;
    private readonly ILogger<TelegramUpdatesReceiverServiceBase<TUpdateHandler>> _logger;

    /// <summary> Конструктор получателя обновлений из Telegram </summary>
    public TelegramUpdatesReceiverServiceBase(
        ITelegramBotClient botClient,
        Telegram.Bot.Polling.IUpdateHandler updateHandler,
        ILogger<TelegramUpdatesReceiverServiceBase<TUpdateHandler>> logger)
    {
        _botClient = botClient;
        _updateHandler = updateHandler;
        _logger = logger;
    }

    /// <summary> Получение обновлений из Telegram </summary>
    public async Task ReceiveUpdates(CancellationToken cancellationToken = default)
    {
        var receiverOptions = new Telegram.Bot.Polling.ReceiverOptions()
        {
            AllowedUpdates = [],
            DropPendingUpdates = true
        };

        var me = await _botClient.GetMe(cancellationToken);

        _logger.LogInformation("Start polling {BotName}", me.Username);

        await _botClient.ReceiveAsync(_updateHandler, receiverOptions, cancellationToken);
    }
}