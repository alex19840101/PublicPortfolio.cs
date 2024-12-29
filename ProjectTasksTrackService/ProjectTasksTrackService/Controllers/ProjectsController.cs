using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProjectTasksTrackService.API.DtoModels;
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

        [HttpGet(Name = "GetProjects")]
        public IEnumerable<Project> GetProjects()
        {
            throw new NotImplementedException();
        }
    }
}
