namespace TelegramBot.API
{
    /// <summary> Настройки для конфигурирования бота Telegram.Bot.TelegramBotClientOptions </summary>
    public class TelegramBotClientOptionsSettings
    {
        /// <summary> Токен для бота <see cref="Telegram.Bot.TelegramBotClientOptions.Token"/> </summary>    
        public string BotToken { get; init; } = default!;
    }
}
