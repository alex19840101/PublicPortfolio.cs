using System.Threading;
using System.Threading.Tasks;

namespace TelegramBot.API.Interfaces
{
    /// <summary> Получатель обновлений из Telegram </summary>
    public interface ITelegramUpdatesReceiverService
    {
        /// <summary> Получить обновления из Telegram </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ReceiveUpdates(CancellationToken cancellationToken = default);
    }
}
