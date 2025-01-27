using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProjectTasksTrackService.API.Contracts.Dto;
using ProjectTasksTrackService.API.Contracts.Dto.Requests;
using ProjectTasksTrackService.API.Contracts.Dto.Responses;
using ProjectTasksTrackService.API.Contracts.Interfaces;
using ProjectTasksTrackService.Core;
using ProjectTasksTrackService.Core.Results;
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
        [ProducesResponseType(typeof(ImportProjectsResponseDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ImportProjectsResponseDto), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Import(IEnumerable<OldProjectDto> projects)
        {
            List<Project> projectsCollection = [];
            foreach (var project in projects)
            {
                projectsCollection.Add(Project(project));
            }

            var importResult = await _projectsService.Import(projectsCollection);

            if (importResult.StatusCode == HttpStatusCode.BadRequest)
                return new BadRequestObjectResult(new ProblemDetails { Title = importResult.Message });

            if (importResult.StatusCode == HttpStatusCode.Conflict || importResult.ImportedCount == 0)
                return new ConflictObjectResult(new ImportProjectsResponseDto
                {
                    Message = importResult.Message
                });

            return CreatedAtAction(nameof(Import), new ImportProjectsResponseDto
            {
                ImportedCount = importResult.ImportedCount,
                Message = importResult.Message
            });
        }

        /// <summary> Создание проекта </summary>
        [HttpPost("api/v2/Projects/Create")]
        [ProducesResponseType(typeof(CreateProjectResponseDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(MessageResponseDto), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Create(ProjectDto project)
        {
            var createResult = await _projectsService.Create(Project(project));

            if (createResult.StatusCode == HttpStatusCode.BadRequest)
                return new BadRequestObjectResult(new ProblemDetails { Title = createResult.Message });

            if (createResult.StatusCode == HttpStatusCode.Conflict)
                return new ConflictObjectResult(new MessageResponseDto { Message = createResult.Message });

            return Ok(new CreateProjectResponseDto { Id = createResult.Id.Value, Code = project.Code });
        }

        /// <summary> Получение списка проектов </summary>
        [HttpGet("api/v2/Projects/GetProjects")]
        [ProducesResponseType(typeof(ProjectDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IEnumerable<ProjectDto>> GetProjects(
            string codeSubStr = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100,
            bool ignoreCase = true)
        {
            var projectsCollection = await _projectsService.GetProjects(codeSubStr, nameSubStr, skipCount, limitCount, ignoreCase);
            
            if (!projectsCollection.Any())
                return [];

            return projectsCollection.Select(p => ProjectDto(p));
        }

        /// <summary> Получение списка проектов (в старом компактном JSON-формате) для экспорта в старую систему </summary>
        [HttpGet("api/v2/Projects/GetProjectsOldDto")]
        [ProducesResponseType(typeof(OldProjectDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IEnumerable<OldProjectDto>> GetProjectsOldDto(
            string codeSubStr = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100,
            bool ignoreCase = true)
        {
            var projectsCollection = await _projectsService.GetProjects(codeSubStr, nameSubStr, skipCount, limitCount);

            if (!projectsCollection.Any())
                return [];

            return projectsCollection.Select(p => OldProjectDto(p));
        }

        /// <summary> Получение проекта </summary>
        [HttpGet("api/v2/Projects/GetProject")]
        [ProducesResponseType(typeof(ProjectDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(MessageResponseDto), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProject(
            int? id = null,
            string codeSubStr = null,
            string nameSubStr = null,
            bool ignoreCase = true)
        {
            var project = await _projectsService.GetProject(id, codeSubStr, nameSubStr);

            if (project is null)
                return NotFound(new MessageResponseDto { Message = ErrorStrings.PROJECT_NOT_FOUND});

            return Ok(ProjectDto(project));
        }

        /// <summary> Обновление проекта </summary>
        [HttpPost("api/v2/Projects/UpdateProject")]
        [ProducesResponseType(typeof(UpdateResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(UpdateResult), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateProject(ProjectDto projectDto)
        {
            var updateResult = await _projectsService.UpdateProject(Project(projectDto));

            if (updateResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(updateResult);

            if (updateResult.StatusCode == HttpStatusCode.Conflict)
                return new ConflictObjectResult(updateResult);

            return Ok(updateResult);
        }

        /// <summary> Удаление проекта </summary>
        [HttpDelete("api/v2/Projects/DeleteProject")]
        public async Task<string> DeleteProject(DeleteProjectRequestDto deleteProjectRequest)
        {
            return await _projectsService.DeleteProject(
                deleteProjectRequest.Id,
                deleteProjectRequest.ProjectSecretString);
        }

        #region Dto<->Core mappers
        [NonAction]
        private static Project Project(OldProjectDto project) =>
            new Project(
                id: project.Id,
                code: project.Code,
                name: project.Name,
                url: project.Url,
                imageUrl: project.ImageUrl,
                createdDt: project.CreatedDt,
                lastUpdateDt: project.LastUpdateDt
            );

        [NonAction]
        private static Project Project(ProjectDto project) =>
            new Project(
                    id: project.Id,
                    code: project.Code,
                    name: project.Name,
                    url: project.Url,
                    imageUrl: project.ImageUrl,
                    createdDt: project.CreatedDt,
                    lastUpdateDt: project.LastUpdateDt
            );

        [NonAction]
        private static ProjectDto ProjectDto(Project project) =>
            new ProjectDto
            {
                Id = project.Id,
                Code = project.Code,
                Name = project.Name,
                Url = project.Url,
                ImageUrl = project.ImageUrl,
                CreatedDt = project.CreatedDt,
                LastUpdateDt = project.LastUpdateDt
            };

        [NonAction]
        private static OldProjectDto OldProjectDto(Project project) =>
            new OldProjectDto
            { 
                Id = project.Id,
                Code = project.Code,
                Name = project.Name,
                Url = project.Url,
                ImageUrl = project.ImageUrl,
                CreatedDt = project.CreatedDt,
                LastUpdateDt = project.LastUpdateDt
            };
        #endregion Dto<->Core mappers
    }
}
