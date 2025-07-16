namespace NotifierBySms.API
{
    /// <summary> Настройки для бота для отправки SMS-уведомлений </summary>
    public class SmsBotClientOptionsSettings
    {
        /// <summary> Токен для бота для отправки SMS-уведомлений </summary>
        public string? BotToken { get; init; } = null!;
        /// <summary> Строка подключения
        /// <para> Azure: Find your Communication Services resource in the Azure portal </para></summary>
        public string? ConnectionString { get; init; } = null!;

        /// <summary> Имя пользователя-бота для отправки SMS-уведомлений </summary>
        public string? UserName { get; init; } = null!;
        /// <summary> Пароль пользователя-бота для отправки SMS-уведомлений </summary>
        public string? Password { get; init; } = null!;

        /// <summary> Host для отправки SMS-уведомлений </summary>
        public string? HostName { get; init; } = default!;

        /// <summary> Порт для отправки SMS-уведомлений </summary>
        public int? Port { get; init; }

        /// <summary> Использовать ли SSL для отправки E-mail-писем (уведомлений)
        /// <para><see langword="true" /> if the client should make an SSL-wrapped connection to the server; otherwise, <see langword="false" />.</para></summary>
        public bool? UseSsl { get; init; }

        /// <summary> Настройки для бота для отправки SMS-уведомлений </summary>
        public SmsBotClientOptionsSettings(
            string botToken)
        {
            BotToken = botToken;
        }
    }
}
