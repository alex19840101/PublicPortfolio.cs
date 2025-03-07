namespace LiteAuthService.API.Contracts.Dto.Responses
{
    /// <summary> Класс ответа на запрос /Create </summary>
    public class CreateResponseDto
    {
        /// <summary> Числовой идентификатор - номер </summary>
        public int Id { get; set; }
        
        /// <summary> Код ((проекта/подпроекта)) </summary>
        public string Code { get; set; }

        /// <summary>
        /// Секретная строка для удаления
        /// </summary>
        public string SecretString { get; set; }
    }
}
