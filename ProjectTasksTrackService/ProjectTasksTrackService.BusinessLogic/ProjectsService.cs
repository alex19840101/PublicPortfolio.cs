using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectTasksTrackService.Core;
using ProjectTasksTrackService.Core.Repositories;
using ProjectTasksTrackService.Core.Services;

namespace ProjectTasksTrackService.BusinessLogic
{
    public class ProjectsService : IProjectsService
    {
        private readonly IProjectsRepository _projectsRepository;

        public ProjectsService(IProjectsRepository projectsRepository)
        {
            _projectsRepository = projectsRepository;
        }

        public async Task<string> Create(Project project)
        {
            if (string.IsNullOrWhiteSpace(project.Name))
                throw new InvalidOperationException("Project Name should be not empty");
            
            return await _projectsRepository.Add(project);
        }

        public async Task<IEnumerable<Project>> GetProjects()
        {
            return await _projectsRepository.GetProjects();
        }

        public async Task<string> Import(IEnumerable<Project> projects)
        {
            if (projects is null)
                throw new InvalidOperationException("Projects to import should not be null");

            if (!projects.Any())
                throw new InvalidOperationException("Projects to import should contain at least 1 project");

            return await _projectsRepository.Import(projects);
        }
    }
}
