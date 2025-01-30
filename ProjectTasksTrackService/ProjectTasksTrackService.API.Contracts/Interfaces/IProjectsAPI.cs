using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectTasksTrackService.API.Contracts.Dto;
using ProjectTasksTrackService.API.Contracts.Dto.Requests;

namespace ProjectTasksTrackService.API.Contracts.Interfaces
{
    /// <summary> Интерфейс контроллера управления проектами </summary>
    public interface IProjectsAPI
    {
        /// <summary> Импорт проектов (из старой системы проектов) </summary>
        Task<IActionResult> Import(IEnumerable<OldProjectDto> projects);
        /// <summary> Создание проекта </summary>
        Task<IActionResult> Create(ProjectDto project);

        /// <summary> Получение проекта </summary>
        Task<IActionResult> GetProject(
            int? id = null,
            string codeSubStr = null,
            string nameSubStr = null,
            bool ignoreCase = true);

        /// <summary> Получение списка проектов (в старом компактном JSON-формате) для экспорта в старую систему </summary>
        Task<IEnumerable<OldProjectDto>> GetProjectsOldDto(
            string codeSubStr = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100,
            bool ignoreCase = true);

        /// <summary> Получение списка проектов </summary>
        Task<IEnumerable<ProjectDto>> GetProjects(
            string codeSubStr = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100,
            bool ignoreCase = true);
        
        /// <summary> Обновление проекта </summary>
        Task<IActionResult> UpdateProject(ProjectDto projectDto);

        /// <summary> Удаление проекта </summary>
        Task<IActionResult> DeleteProject(DeleteProjectRequestDto deleteProjectRequest);
    }
}
