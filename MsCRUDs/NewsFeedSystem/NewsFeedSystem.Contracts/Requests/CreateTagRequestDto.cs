namespace NewsFeedSystem.API.Contracts.Requests
{
    /// <summary> Запрос на создание тега </summary>
    public class CreateTagRequestDto
    {
        /// <summary> Тег </summary>
        public required string Tag {  get; set; }
    }
}
