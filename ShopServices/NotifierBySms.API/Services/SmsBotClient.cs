using System.Net.Http;
using System.Threading;
using NotifierBySms.API.Interfaces;

namespace NotifierBySms.API.Services
{
    /// <summary> Клиент-бот для отправки SMS-уведомлений </summary>
    public class SmsBotClient : ISmsBotClient
    {
        /// <summary> Настройки для бота для отправки SMS-уведомлений </summary>
        private readonly SmsBotClientOptionsSettings _settings;

        private readonly HttpClient? _httpClient;

        /// <summary> Токен для бота для отправки SMS-уведомлений </summary>
        public string? Token => _settings.BotToken;

        /// <summary> Global cancellation token </summary>
        public CancellationToken GlobalCancelToken { get; }

        /// <summary> Клиент-бот для отправки SMS-уведомлений </summary>
        /// <param name="settings"> Настройки для бота для отправки SMS-уведомлений </param>
        /// <param name="httpClient"> (Если использовать HttpClient) | A custom <see cref="HttpClient"/> </param>
        /// <param name="cancellationToken"> Global cancellation token </param>
        public SmsBotClient(SmsBotClientOptionsSettings settings,
            HttpClient? httpClient = default,
            CancellationToken cancellationToken = default)
        {
            _settings = settings;
            _httpClient = httpClient;
            GlobalCancelToken = cancellationToken;
        }
    }
}
