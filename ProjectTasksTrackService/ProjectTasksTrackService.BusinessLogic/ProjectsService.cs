using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectTasksTrackService.Contracts;
using ProjectTasksTrackService.Contracts.Repositories;
using ProjectTasksTrackService.Contracts.Services;

namespace ProjectTasksTrackService.BusinessLogic
{
    public class ProjectsService : IProjectsService
    {
        private readonly IProjectsRepository _projectsRepository;

        public ProjectsService(IProjectsRepository projectsRepository)
        {
            _projectsRepository = projectsRepository;
        }

        public async Task<Project> Create(Project project)
        {
            if (string.IsNullOrWhiteSpace(project.Name))
                throw new InvalidOperationException("Project Name should be not empty");
            
            return await _projectsRepository.Add(project);
        }

        public async Task<IEnumerable<Project>> GetProjects()
        {
            return await _projectsRepository.GetProjects();
        }
    }
}
