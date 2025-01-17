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

        public async Task<int> Create(Project project)
        {
            if (string.IsNullOrWhiteSpace(project.ProjectId))
                throw new InvalidOperationException(ErrorStrings.PROJECT_ID_SHOULD_NOT_BE_EMPTY);

            if (string.IsNullOrWhiteSpace(project.Name))
                throw new InvalidOperationException(ErrorStrings.PROJECT_NAME_SHOULD_NOT_BE_EMPTY);

            var intProjectId = await _projectsRepository.Add(project);

            return intProjectId;
        }

        public async Task<int> Import(IEnumerable<Project> projects)
        {
            if (projects is null)
                throw new InvalidOperationException(ErrorStrings.PROJECTS_LIST_TO_IMPORT_SHOULD_NOT_BE_NULL);

            if (!projects.Any())
                throw new InvalidOperationException(ErrorStrings.PROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED);

            return await _projectsRepository.Import(projects);
        }

        public Task<string> UpdateProject(Project projectDto)
        {
            if (string.IsNullOrWhiteSpace(projectDto.ProjectId))
                throw new InvalidOperationException(ErrorStrings.PROJECT_ID_SHOULD_NOT_BE_EMPTY);

            if (string.IsNullOrWhiteSpace(projectDto.Name))
                throw new InvalidOperationException(ErrorStrings.PROJECT_NAME_SHOULD_NOT_BE_EMPTY);

            throw new NotImplementedException();
        }
        public Task<string> DeleteProject(string projectId, string projectSecretString)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Project>> GetProjects(
            string projectId = null,
            int? intProjectId = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100)
        {
            throw new NotImplementedException();
        }

        public Task<Project> GetProject(string projectId = null, int? intProjectId = null, string name = null)
        {
            throw new NotImplementedException();
        }
    }
}
