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

        /// <summary> Получение подпроекта </summary>
        Task<ProjectSubDivisionDto> GetSubDivision(string projectId, int subDivisionId);

        /// <summary> Получение списка подпроектов </summary>
        Task<IEnumerable<ProjectSubDivisionDto>> GetSubDivisions(
            string projectId = null,
            int? intProjectId = null,
            int? subDivisionId = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100);

        /// <summary> Получение списка подпроектов (в старом компактном JSON-формате) для экспорта в старую систему </summary>
        Task<IEnumerable<OldProjectSubDivisionDto>> GetSubDivisionsOldDto(
            string projectId = null,
            int? intProjectId = null,
            int? subDivisionId = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100);

        /// <summary> Получение списка актуальных подпроектов </summary>
        Task<IEnumerable<ProjectSubDivisionDto>> GetHotSubDivisions(
            string projectId = null,
            DateTime? deadLine = null,
            int skipCount = 0,
            int limitCount = 100);

        /// <summary> Обновление подпроекта </summary>
        Task<string> UpdateSubDivision(ProjectSubDivisionDto taskDto);

        /// <summary> Удаление подпроекта </summary>
        Task<string> DeleteSubDivision(DeleteProjectSubDivisionDto deleteSubProjectRequest);
    }
}
