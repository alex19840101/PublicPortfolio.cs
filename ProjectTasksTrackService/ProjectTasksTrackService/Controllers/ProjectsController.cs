using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectTasksTrackService.Contracts;
using ProjectTasksTrackService.Contracts.Services;

namespace ProjectTasksTrackService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsService _projectsService;
        public ProjectsController(IProjectsService projectsService)
        {
            _projectsService = projectsService;
        }

        [HttpPost(Name = "Create")]
        public async Task<Project> Create(Project project)
        {
            return await _projectsService.Create(project);
        }

        [HttpGet(Name = "GetProjects")]
        public async Task<IEnumerable<Project>> Read()
        {
            return await _projectsService.GetProjects();
        }
    }
}
