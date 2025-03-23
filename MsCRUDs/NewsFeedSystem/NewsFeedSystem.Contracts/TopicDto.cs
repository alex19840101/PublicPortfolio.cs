namespace NewsFeedSystem.API.Contracts
{
    /// <summary>
    /// Класс новостной темы
    /// </summary>
    public class TopicDto
    {
        /// <summary>
        /// Id темы (при создании не указывается)
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Тема
        /// </summary>
        public required string Topic { get; set; }
    }
}
