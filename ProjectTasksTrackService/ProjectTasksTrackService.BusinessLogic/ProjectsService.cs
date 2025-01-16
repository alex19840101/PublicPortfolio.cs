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
            if (string.IsNullOrWhiteSpace(project.ProjectId))
                throw new InvalidOperationException(ErrorStrings.PROJECT_ID_SHOULD_NOT_BE_EMPTY);

            if (string.IsNullOrWhiteSpace(project.Name))
                throw new InvalidOperationException(ErrorStrings.PROJECT_NAME_SHOULD_NOT_BE_EMPTY);

            return await _projectsRepository.Add(project);
        }

        public Task<string> DeleteProject(string projectId, string projectSecretString)
        {
            throw new NotImplementedException();
        }

        public Task<Project> GetProjectById(string projectId)
        {
            throw new NotImplementedException();
        }

        public Task<Project> GetProjectByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Project> GetProjectByNum(int intProjectId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Project>> GetProjects()
        {
            return await _projectsRepository.GetProjects();
        }

        public async Task<string> Import(IEnumerable<Project> projects)
        {
            if (projects is null)
                throw new InvalidOperationException(ErrorStrings.PROJECTS_LIST_TO_IMPORT_SHOULD_NOT_BE_NULL);

            if (!projects.Any())
                throw new InvalidOperationException(ErrorStrings.PROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED);

            return await _projectsRepository.Import(projects);
        }

        public Task<string> UpdateImageUrl(string projectId, string imageUrl)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateName(string projectId, string newName)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateProject(Project projectDto)
        {
            if (string.IsNullOrWhiteSpace(projectDto.ProjectId))
                throw new InvalidOperationException(ErrorStrings.PROJECT_ID_SHOULD_NOT_BE_EMPTY);

            if (string.IsNullOrWhiteSpace(projectDto.Name))
                throw new InvalidOperationException(ErrorStrings.PROJECT_NAME_SHOULD_NOT_BE_EMPTY);

            throw new NotImplementedException();
        }

        public Task<string> UpdateScheduledDayNums(string projectId, HashSet<byte> scheduledDayNums)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateUrl(string projectId, string url)
        {
            throw new NotImplementedException();
        }
    }
}
