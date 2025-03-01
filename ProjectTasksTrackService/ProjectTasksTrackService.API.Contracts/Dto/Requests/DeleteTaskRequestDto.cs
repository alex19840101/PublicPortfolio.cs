namespace ProjectTasksTrackService.API.Contracts.Dto.Requests
{
    /// <summary>
    /// Запрос на удаление задачи
    /// </summary>
    public class DeleteTaskRequestDto
    {
        /// <summary>
        /// Id задачи
        /// </summary>
        public int TaskId { get; set; }
        /// <summary>
        /// Секретная строка для удаления задачи
        /// </summary>
        public string TaskSecretString { get; set; }
    }
}
