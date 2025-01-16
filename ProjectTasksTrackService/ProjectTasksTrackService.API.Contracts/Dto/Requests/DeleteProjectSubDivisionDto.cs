namespace ProjectTasksTrackService.API.Contracts.Dto.Requests
{
    /// <summary>
    /// Запрос на удаление подпроекта
    /// </summary>
    public class DeleteProjectSubDivisionDto
    {
        /// <summary>
        /// Id проекта
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// Id подпроекта
        /// </summary>
        public int SubDivisionId { get; set; }
        /// <summary>
        /// Секретная строка для удаления подпроекта
        /// </summary>
        public string SubDivisionSecretString { get; set; }
    }
}
