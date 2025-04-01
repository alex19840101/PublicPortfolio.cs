namespace NewsFeedSystem.API.Contracts.Responses
{
    /// <summary>
    /// Класс ответа с сообщением о предупреждении/ошибке
    /// </summary>
    public class MessageResponseDto
    {
        /// <summary>
        /// Сообщение о предупреждении/ошибке
        /// </summary>
        public string Message { get; set; } = default!;
    }
}
