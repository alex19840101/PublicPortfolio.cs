using Microsoft.Extensions.Logging;

namespace TelegramBot.API.Services
{
    /// <summary> Получатель обновлений из Telegram </summary>
    public class TelegramUpdatesReceiverService(
        Telegram.Bot.ITelegramBotClient botClient,
        TelegramUpdateHandler updateHandler,
        ILogger<TelegramUpdatesReceiverServiceBase<TelegramUpdateHandler>> logger) :
        TelegramUpdatesReceiverServiceBase<TelegramUpdateHandler>(botClient, updateHandler, logger);
}
