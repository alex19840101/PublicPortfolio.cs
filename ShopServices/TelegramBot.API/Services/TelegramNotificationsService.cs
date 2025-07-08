using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using TelegramBot.API.Interfaces;

namespace TelegramBot.API.Services
{
    /// <summary> Нотификатор, отправляющий Telegram-уведомления </summary>
    public class TelegramNotificationsService : ITelegramNotificationService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<TelegramNotificationsService> _logger;

        /// <summary> Конструктор нотификатора, отправляющего Telegram-уведомления </summary>
        public TelegramNotificationsService(
            ITelegramBotClient botClient,
            ILogger<TelegramNotificationsService> logger)
        {
            _botClient = botClient;
            _logger = logger;
        }

        /// <summary> Отправка уведомления в Telegram </summary>
        /// <param name="chatId"></param>
        /// <param name="text"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Telegram.Bot.Types.Message> SendMessage(
            Telegram.Bot.Types.ChatId chatId,
            string text,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _botClient.SendMessage(chatId, text, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR SendMessage {ChatId}", chatId.Identifier);
                throw;
            }
        }
    }
}
