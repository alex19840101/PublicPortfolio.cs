
namespace ProjectTasksTrackService.API.Contracts.Dto.Responses
{
    /// <summary> Класс ответа на запрос /Projects/Import </summary>
    public class ImportResponseDto
    {
        /// <summary> Количество импортированных (добавленных) проектов </summary>
        public int ImportedCount { get; set; }

        /// <summary> Сообщение о результате импорта </summary>
        public string Message { get; set; }
    }
}
