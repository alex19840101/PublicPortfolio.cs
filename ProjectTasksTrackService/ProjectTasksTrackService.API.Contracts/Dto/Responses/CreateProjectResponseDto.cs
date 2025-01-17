namespace ProjectTasksTrackService.API.Contracts.Dto.Responses
{
    /// <summary> Класс ответа на запрос /Projects/Create </summary>
    public class CreateProjectResponseDto
    {
        /// <summary> Id проекта </summary>
        public string ProjectId { get; set; }
        
        /// <summary> Числовой идентификатор (номер) проекта, как в старой системе </summary>
        public int IntProjectId { get; set; }
    }
}
