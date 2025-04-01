namespace NewsFeedSystem.API.Contracts.Requests
{
    /// <summary> Запрос на создание темы </summary>
    public class CreateTopicRequestDto
    {
        /// <summary> Тема </summary>
        public required string Topic {  get; set; }
    }
}
