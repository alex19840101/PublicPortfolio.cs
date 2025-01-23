using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTasksTrackService.API.Contracts.Dto.Responses
{
    /// <summary>
    /// Класс ответа с сообщением о предупреждении/ошибке
    /// </summary>
    public class MessageResponseDto
    {
        /// <summary>
        /// Ссообщение о предупреждении/ошибке
        /// </summary>
        public string Message { get; set; }
    }
}
