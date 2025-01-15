using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string ProjectId { get; set; }
        /// <summary>
        /// Секретная строка для удаления проекта
        /// </summary>
        public string ProjectSecretString { get; set; }
    }
}
