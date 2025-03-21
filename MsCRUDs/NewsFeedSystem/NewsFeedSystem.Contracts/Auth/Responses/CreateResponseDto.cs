namespace NewsFeedSystem.API.Contracts.Auth.Responses
{
    /// <summary> Класс ответа на запрос /Create </summary>
    public class CreateResponseDto
    {
        /// <summary> Числовой идентификатор - номер </summary>
        public int Id { get; set; }

        /// <summary> Сообщение о результате выполнения запроса </summary>
        public string Message { get; set; }
    }
}
