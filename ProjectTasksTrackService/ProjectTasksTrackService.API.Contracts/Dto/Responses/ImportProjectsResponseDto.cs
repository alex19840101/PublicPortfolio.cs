
namespace ProjectTasksTrackService.API.Contracts.Dto.Responses
{
    /// <summary> Класс ответа на запрос /Projects/Import </summary>
    public class ImportProjectsResponseDto
    {
        /// <summary> Количество импортированных (добавленных) проектов </summary>
        public int ImportedCount { get; set; }
    }
}
