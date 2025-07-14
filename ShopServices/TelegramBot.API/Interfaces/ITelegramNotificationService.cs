using System.Threading;
using System.Threading.Tasks;

namespace TelegramBot.API.Interfaces
{
    /// <summary> Нотификатор, отправляющий Telegram-уведомления </summary>
    public interface ITelegramNotificationService
    {
        /// <summary> Отправка уведомления в Telegram </summary>
        /// <param name="chatId"></param>
        /// <param name="text"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Telegram.Bot.Types.Message?> SendMessage(
            Telegram.Bot.Types.ChatId chatId,
            string text,
            CancellationToken cancellationToken = default);
    }
}
