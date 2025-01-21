namespace ProjectTasksTrackService.API.Contracts.Dto.Responses
{
    /// <summary> Класс ответа на запрос /Projects/Create </summary>
    public class CreateProjectResponseDto
    {
        /// <summary> Числовой идентификатор - номер проекта, как в старой системе </summary>
        public int Id { get; set; }
        
        /// <summary> Код проекта </summary>
        public string Code { get; set; }

        /// <summary>
        /// Секретная строка для удаления проекта
        /// </summary>
        public string ProjectSecretString { get; set; }
    }
}
