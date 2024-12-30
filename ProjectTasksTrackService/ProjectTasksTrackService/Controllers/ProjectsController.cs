using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectTasksTrackService.API.Contracts.Dto;
using ProjectTasksTrackService.API.Contracts.Interfaces;
using ProjectTasksTrackService.Core;
using ProjectTasksTrackService.Core.Services;

namespace ProjectTasksTrackService.API.Controllers
{
    [ApiController]
    [Route("ProjectTasksTrackService")]
    public class ProjectsController : ControllerBase, IProjectsAPI
    {
        private readonly IProjectsService _projectsService;
        public ProjectsController(IProjectsService projectsService)
        {
            _projectsService = projectsService;
        }

        [HttpPost("api/Projects/Create")]
        public async Task<string> Create(ProjectDto project)
        {
            return await _projectsService.Create(Project(project));
        }

        [HttpGet("api/Projects/GetProjects")]
        public async Task<IEnumerable<ProjectDto>> GetProjects()
        {
            var projectsCollection = await _projectsService.GetProjects();
            List<ProjectDto> result = [];
            foreach (var project in projectsCollection)
            {
                result.Add(ProjectDto(project));
            }

            return result;
        }

        [HttpPost("api/Projects/Import")]
        public async Task<string> Import(IEnumerable<OldProjectDto> projects)
        {
            List<Project> projectsCollection = [];
            foreach (var project in projects)
            {
                projectsCollection.Add(Project(project));
            }

            return await _projectsService.Import(projectsCollection);
        }

        [HttpGet("api/Projects/GetProjectById")]
        public async Task<ProjectDto> GetProjectById(string projectId)
        {
            var project = await _projectsService.GetProjectById(projectId);
            return ProjectDto(project);
        }

        [HttpGet("api/Projects/GetProjectByNum")]
        public async Task<ProjectDto> GetProjectByNum(int legacyProjectNumber)
        {
            var project = await _projectsService.GetProjectByNum(legacyProjectNumber);
            return ProjectDto(project);
        }

        [HttpGet("api/Projects/GetProjectByName")]
        public async Task<ProjectDto> GetProjectByName(string name)
        {
            var project = await _projectsService.GetProjectByName(name);
            return ProjectDto(project);
        }
        
        [HttpPost("api/Projects/UpdateProject")]
        public async Task<string> UpdateProject(ProjectDto projectDto)
        {
            return await _projectsService.UpdateProject(Project(projectDto));
        }

        [HttpPost("api/Projects/UpdateName")]
        public async Task<string> UpdateName(string projectId, string newName)
        {
            return await _projectsService.UpdateName(projectId, newName);
        }

        [HttpPost("api/Projects/UpdateUrl")]
        public async Task<string> UpdateUrl(string projectId, string url)
        {
            return await _projectsService.UpdateUrl(projectId, url);
        }

        [HttpPost("api/Projects/UpdateImageUrl")]
        public async Task<string> UpdateImageUrl(string projectId, string imageUrl)
        {
            return await _projectsService.UpdateImageUrl(projectId, imageUrl);
        }

        [HttpPost("api/Projects/UpdateScheduledDayNums")]
        public async Task<string> UpdateScheduledDayNums(string projectId, HashSet<byte> scheduledDayNums)
        {
            return await _projectsService.UpdateScheduledDayNums(projectId, scheduledDayNums);
        }

        [HttpPost("api/Projects/DeleteProject")]
        public async Task<string> DeleteProject(string projectId, string projectSecretString)
        {
            return await _projectsService.DeleteProject(projectId, projectSecretString);
        }


        [NonAction]
        private static Project Project(OldProjectDto project) =>
            new Project(
                projectId: project.ProjectId,
                name: project.Name,
                legacyProjectNumber: project.LegacyProjectNumber,
                url: project.URL,
                imageUrl: project.ImageURL,
                scheduledDayNums: project.ScheduledDayNums
        );

        [NonAction]
        private static Project Project(ProjectDto project) =>
            new Project(
                    projectId: project.ProjectId,
                    name: project.Name,
                    legacyProjectNumber: project.LegacyProjectNumber,
                    url: project.Url,
                    imageUrl: project.ImageUrl,
                    scheduledDayNums: project.ScheduledDayNums
            );

        [NonAction]
        private static ProjectDto ProjectDto(Project project) =>
            new ProjectDto
            {
                ProjectId = project.ProjectId,
                Name = project.Name,
                LegacyProjectNumber = project.LegacyProjectNumber.Value,
                Url = project.Url,
                ImageUrl = project.ImageURL,
                ScheduledDayNums = project.ScheduledDayNums
            };
    }
}
