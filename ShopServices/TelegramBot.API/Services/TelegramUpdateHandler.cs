using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using TelegramBot.API.Interfaces;

namespace TelegramBot.API.Services;

/// <summary> Обработчик обновлений из Telegram </summary>
public class TelegramUpdateHandler : IUpdateHandler
{
    private readonly ITelegramNotificationService _telegramNotificationService;
    private readonly ILogger<TelegramUpdateHandler> _logger;

    /// <summary> Конструктор обработчика обновлений из Telegram </summary>
    public TelegramUpdateHandler(
        ITelegramNotificationService telegramService,
        ILogger<TelegramUpdateHandler> logger)
    {
        _telegramNotificationService = telegramService;
        _logger = logger;
    }

    /// <summary> Обработчик обновлений из Telegram для Telegram.Bot.
    /// <para>Handles an <see cref="Update"/></para></summary>
    /// <param name="botClient">The <see cref="ITelegramBotClient"/> instance of the bot receiving the <see cref="Update"/></param>
    /// <param name="update">The <see cref="Update"/> to handle</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> which will notify that method execution should be cancelled</param>
    public async Task HandleUpdateAsync(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await (update switch
        {
            { Message: { } message } => OnMessage(message),
            _ => UnknownUpdateTypeHandler(update)
        });
    }

    /// <summary> Обработчик ошибки для Telegram.Bot
    /// <para>Handles an <see cref="Exception"/></para></summary>
    /// <param name="botClient">The <see cref="ITelegramBotClient"/> instance of the bot receiving the <see cref="Exception"/></param>
    /// <param name="exception">The <see cref="Exception"/> to handle</param>
    /// <param name="source">Where the error occured</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> which will notify that method execution should be cancelled</param>
    public Task HandleErrorAsync(
        ITelegramBotClient botClient,
        Exception exception,
        HandleErrorSource source,
        CancellationToken cancellationToken) =>
        throw new NotImplementedException();


    /// <summary> Обработчик нового Telegram-сообщения </summary>
    /// <param name="message"> Telegram.Bot.Types.Message-данные сообщения </param>
    /// <returns></returns>
    private async Task OnMessage(Message message)
    {
        _logger.LogInformation("Receive message type: {MessageType}", message.Type);

        if (string.IsNullOrWhiteSpace(message.Text))
            return;

        await _telegramNotificationService.SendMessage(message.Chat, $"RECEIVED MESSAGE: {message.Text}");
    }

    /// <summary> Обработчик неизвестного типа обновлений из Telegram </summary>
    /// <param name="update"> Данные об обновлении в Telegram </param>
    /// <returns></returns>
    private Task UnknownUpdateTypeHandler(Update update)
    {
        _logger.LogInformation("Unknown update type {UpdateType}:", update.Type);

        return Task.CompletedTask;
    }

    //public async Task HandlePollingErrorAsync(
    //    ITelegramBotClient botClient,
    //    Exception exception,
    //    CancellationToken cancellationToken)
    //{
    //    _logger.LogInformation("HandleError: {exception}", exception);

    //    if (exception is RequestException)
    //        await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
    //}
}