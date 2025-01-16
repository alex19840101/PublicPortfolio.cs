using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectTasksTrackService.API.Contracts.Dto;
using ProjectTasksTrackService.API.Contracts.Dto.Requests;

namespace ProjectTasksTrackService.API.Contracts.Interfaces
{
    /// <summary> Интерфейс контроллера управления подпроектами </summary>
    public interface IProjectSubDivisionAPI
    {
        /// <summary> Импорт подпроектов (из старой системы) </summary>
        Task<string> Import(IEnumerable<OldProjectSubDivisionDto> subprojects);
        
        /// <summary> Создание подпроекта </summary>
        Task<string> Create(ProjectSubDivisionDto subproject);

        /// <summary> Обновление подпроекта </summary>
        Task<string> UpdateSubDivision(ProjectSubDivisionDto taskDto);

        /// <summary> Удаление подпроекта </summary>
        Task<string> DeleteSubDivision(DeleteProjectSubDivisionDto deleteSubProjectRequest);
    }
}
