namespace NotifierByEmail.API
{
    /// <summary> Настройки бота для отправки E-mail-писем (уведомлений) </summary>    
    public class EmailBotOptionsSettings
    {
        /// <summary> Токен для бота для отправки E-mail-писем (уведомлений) </summary>
        public string? BotToken { get; init; } = null!;
        /// <summary> Имя пользователя-бота для отправки E-mail-писем (уведомлений) </summary>
        public string? UserName { get; init; } = null!;
        /// <summary> Пароль пользователя-бота для отправки E-mail-писем (уведомлений) </summary>
        public string? Password { get; init; } = null!;

        /// <summary> Host для отправки E-mail-писем (уведомлений) </summary>
        public string HostName { get; init; } = default!;

        /// <summary> Порт для отправки E-mail-писем (уведомлений) </summary>
        public int Port { get; init; }

        /// <summary> Использовать ли SSL для отправки E-mail-писем (уведомлений)
        /// <para><see langword="true" /> if the client should make an SSL-wrapped connection to the server; otherwise, <see langword="false" />.</para></summary>
        public bool UseSsl { get; init; }
    }
}
