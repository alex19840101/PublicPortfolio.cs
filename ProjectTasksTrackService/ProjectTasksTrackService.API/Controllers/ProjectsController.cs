using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectTasksTrackService.API.Contracts.Dto;
using ProjectTasksTrackService.API.Contracts.Dto.Requests;
using ProjectTasksTrackService.API.Contracts.Interfaces;
using ProjectTasksTrackService.Core;
using ProjectTasksTrackService.Core.Services;

namespace ProjectTasksTrackService.API.Controllers
{
    /// <summary> Контроллер управления проектами </summary>
    [ApiController]
    [Route("ProjectTasksTrackService")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class ProjectsController : ControllerBase, IProjectsAPI
    {
        private readonly IProjectsService _projectsService;
        /// <summary> </summary>
        public ProjectsController(IProjectsService projectsService)
        {
            _projectsService = projectsService;
        }

        /// <summary> Импорт проектов (из старой системы проектов) </summary>
        [HttpPost("api/v2/Projects/Import")]
        public async Task<string> Import(IEnumerable<OldProjectDto> projects)
        {
            List<Project> projectsCollection = [];
            foreach (var project in projects)
            {
                projectsCollection.Add(Project(project));
            }

            return await _projectsService.Import(projectsCollection);
        }

        /// <summary> Создание проекта </summary>
        [HttpPost("api/v2/Projects/Create")]
        public async Task<string> Create(ProjectDto project)
        {
            return await _projectsService.Create(Project(project));
        }

        /// <summary> Получение списка проектов </summary>
        [HttpGet("api/v2/Projects/GetProjects")]
        public async Task<IEnumerable<ProjectDto>> GetProjects(string projectId = null, int? intProjectId = null, string nameSubStr = null)
        {
            var projectsCollection = await _projectsService.GetProjects(projectId, intProjectId, nameSubStr);
            List<ProjectDto> result = [];
            foreach (var project in projectsCollection)
            {
                result.Add(ProjectDto(project));
            }

            return result;
        }

        /// <summary> Получение списка проектов (в старом компактном JSON-формате) для экспорта в старую систему </summary>
        [HttpGet("api/v2/Projects/GetProjectsOldDto")]
        public async Task<IEnumerable<OldProjectDto>> GetProjectsOldDto(string projectId = null, int? intProjectId = null, string nameSubStr = null)
        {
            var projectsCollection = await _projectsService.GetProjects(projectId, intProjectId, nameSubStr);
            List<OldProjectDto> result = [];
            foreach (var project in projectsCollection)
            {
                result.Add(OldProjectDto(project));
            }

            return result;
        }

        /// <summary> Получение проекта </summary>
        [HttpGet("api/v2/Projects/GetProject")]
        public async Task<ProjectDto> GetProject(string projectId = null, int? intProjectId = null, string name = null)
        {
            var project = await _projectsService.GetProject(projectId, intProjectId, name);
            return ProjectDto(project);
        }

        /// <summary> Обновление проекта </summary>
        [HttpPost("api/v2/Projects/UpdateProject")]
        public async Task<string> UpdateProject(ProjectDto projectDto)
        {
            return await _projectsService.UpdateProject(Project(projectDto));
        }

        /// <summary> Удаление проекта </summary>
        [HttpDelete("api/v2/Projects/DeleteProject")]
        public async Task<string> DeleteProject(DeleteProjectRequestDto deleteProjectRequest)
        {
            return await _projectsService.DeleteProject(
                deleteProjectRequest.ProjectId,
                deleteProjectRequest.ProjectSecretString);
        }

        #region Dto<->Core mappers
        [NonAction]
        private static Project Project(OldProjectDto project) =>
            new Project(
                projectId: project.ProjectId,
                name: project.Name,
                intProjectId: project.IntProjectId,
                url: project.Url,
                imageUrl: project.ImageUrl,
                scheduledDayNums: project.ScheduledDayNums,
                createdDt: project.CreatedDt,
                lastUpdateDt: project.LastUpdateDt
            );

        [NonAction]
        private static Project Project(ProjectDto project) =>
            new Project(
                    projectId: project.ProjectId,
                    name: project.Name,
                    intProjectId: project.IntProjectId,
                    url: project.Url,
                    imageUrl: project.ImageUrl,
                    scheduledDayNums: project.ScheduledDayNums,
                    createdDt: project.CreatedDt,
                    lastUpdateDt: project.LastUpdateDt
            );

        [NonAction]
        private static ProjectDto ProjectDto(Project project) =>
            new ProjectDto
            {
                ProjectId = project.ProjectId,
                Name = project.Name,
                IntProjectId = project.intProjectId.Value,
                Url = project.Url,
                ImageUrl = project.ImageUrl,
                ScheduledDayNums = project.ScheduledDayNums,
                CreatedDt = project.CreatedDt,
                LastUpdateDt = project.LastUpdateDt
            };

        [NonAction]
        private static OldProjectDto OldProjectDto(Project project) =>
            new OldProjectDto
            { 
                ProjectId = project.ProjectId,
                Name = project.Name,
                IntProjectId = project.intProjectId.Value,
                Url = project.Url,
                ImageUrl = project.ImageUrl,
                ScheduledDayNums = project.ScheduledDayNums,
                CreatedDt = project.CreatedDt,
                LastUpdateDt = project.LastUpdateDt
            };
        #endregion Dto<->Core mappers
    }
}
