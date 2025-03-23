namespace NewsFeedSystem.API.Contracts
{
    /// <summary>
    /// Класс тега
    /// </summary>
    public class TagDto
    {
        /// <summary>
        /// Id тега (при создании не указывается)
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Тег
        /// </summary>
        public required string Tag { get; set; }
    }
}
