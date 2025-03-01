namespace ProjectTasksTrackService.API.Contracts.Dto.Requests
{
    /// <summary>
    /// Запрос на удаление проекта
    /// </summary>
    public class DeleteProjectRequestDto
    {
        /// <summary>
        /// Id проекта
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Секретная строка для удаления проекта
        /// </summary>
        public string ProjectSecretString { get; set; }
    }
}
