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
        Task<ProjectDto> GetProject(string projectId = null, int? intProjectId = null, string name = null);

        /// <summary> Получение списка проектов (в старом компактном JSON-формате) для экспорта в старую систему </summary>
        Task<IEnumerable<OldProjectDto>> GetProjectsOldDto(
            string projectId = null,
            int? intProjectId = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100);

        /// <summary> Получение списка проектов </summary>
        Task<IEnumerable<ProjectDto>> GetProjects(
            string projectId = null,
            int? intProjectId = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100);
        
        /// <summary> Обновление проекта </summary>
        Task<string> UpdateProject(ProjectDto projectDto);

        /// <summary> Удаление проекта </summary>
        Task<string> DeleteProject(DeleteProjectRequestDto deleteProjectRequest);
    }
}
