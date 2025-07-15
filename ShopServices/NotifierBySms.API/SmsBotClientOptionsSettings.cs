namespace NotifierBySms.API
{
    /// <summary> Настройки для бота для отправки SMS-уведомлений </summary>
    public class SmsBotClientOptionsSettings
    {
        /// <summary> Токен для бота для отправки SMS-уведомлений </summary>
        public string? BotToken { get; init; } = null!;

        /// <summary> Настройки для бота для отправки SMS-уведомлений </summary>
        public SmsBotClientOptionsSettings(
            string botToken)
        {
            BotToken = botToken;
        }
    }
}
