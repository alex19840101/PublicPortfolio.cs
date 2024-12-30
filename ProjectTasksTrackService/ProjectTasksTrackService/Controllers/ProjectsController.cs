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
        public Task<ProjectDto> GetProjectById(string projectId)
        {
            throw new System.NotImplementedException();
        }

        [HttpGet("api/Projects/GetProjectByNum")]
        public Task<ProjectDto> GetProjectByNum(int legacyProjectNumber)
        {
            throw new System.NotImplementedException();
        }

        [HttpGet("api/Projects/GetProjectByName")]
        public Task<ProjectDto> GetProjectByName(string name)
        {
            throw new System.NotImplementedException();
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
