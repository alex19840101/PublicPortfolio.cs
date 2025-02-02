using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectTasksTrackService.Core;
using ProjectTasksTrackService.Core.Repositories;
using ProjectTasksTrackService.Core.Results;
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

        public async Task<CreateResult> Create(Project project)
        {
            if (project == null)
                throw new ArgumentNullException(ErrorStrings.PROJECT_PARAM_NAME);

            if (string.IsNullOrWhiteSpace(project.Code))
                return new CreateResult(ErrorStrings.PROJECT_CODE_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(project.Name))
                return new CreateResult(ErrorStrings.PROJECT_NAME_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (project.Id != 0)
                return new CreateResult(ErrorStrings.PROJECT_ID_SHOULD_BE_ZERO, System.Net.HttpStatusCode.BadRequest);

            var createResult = await _projectsRepository.Add(project);

            return createResult;
        }

        public async Task<ImportResult> Import(IEnumerable<Project> projects)
        {
            if (projects is null)
                throw new ArgumentNullException(ErrorStrings.PROJECTS_PARAM_NAME);

            if (!projects.Any())
                return new ImportResult(ErrorStrings.PROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED, System.Net.HttpStatusCode.BadRequest);

            var existingProjects = await _projectsRepository.GetAllProjects();

            List<int> conflictedIds = new List<int>();
            List<Project> newProjectsToImport = new List<Project>();
            foreach (var impProject in projects)
            {
                if (existingProjects.Any(p => p.Id == impProject.Id))
                {
                    var existingProject = existingProjects.Single(p => p.Id == impProject.Id);
                    if (!impProject.Equals(existingProject))
                        conflictedIds.Add(impProject.Id);
                }
                else
                    newProjectsToImport.Add(impProject);
            }

            if (conflictedIds.Any())
                return new ImportResult { ImportedCount = 0, Message = $"{ErrorStrings.PROJECT_CONFLICTS}:{string.Join(",", conflictedIds)}" };

            if (!newProjectsToImport.Any())
                return new ImportResult { ImportedCount = 0, Message = ErrorStrings.ALREADY_IMPORTED, StatusCode = System.Net.HttpStatusCode.OK };

            var importResult = await _projectsRepository.Import(newProjectsToImport);

            return new ImportResult { ImportedCount = importResult.ImportedCount, Message = ErrorStrings.IMPORTED };
        }

        public async Task<UpdateResult> UpdateProject(Project project)
        {
            if (string.IsNullOrWhiteSpace(project.Code))
                return new UpdateResult
                {
                    Message = ErrorStrings.PROJECT_CODE_SHOULD_NOT_BE_EMPTY,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

            if (string.IsNullOrWhiteSpace(project.Name))
                return new UpdateResult
                {
                    Message = ErrorStrings.PROJECT_NAME_SHOULD_NOT_BE_EMPTY,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

            return await _projectsRepository.UpdateProject(project);
        }
        public async Task<DeleteResult> DeleteProject(int id, string projectSecretString)
        {
            return await _projectsRepository.DeleteProject(id, projectSecretString);
        }

        public async Task<IEnumerable<Project>> GetProjects(
            string codeSubStr = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100,
            bool ignoreCase = true)
        {
            return await _projectsRepository.GetProjects(codeSubStr, nameSubStr, skipCount, limitCount);
        }

        public async Task<Project> GetProject(
            int? id = null,
            string codeSubStr = null,
            string nameSubStr = null,
            bool ignoreCase = true)
        {
            bool anyCode = string.IsNullOrWhiteSpace(codeSubStr);
            bool anyName = string.IsNullOrWhiteSpace(nameSubStr);

            if (id == null && anyCode && anyName)
                throw new InvalidOperationException(ErrorStrings.GET_PROJECT_CALLED_WITH_NULL_EMPTY_PRMS);
            
            if (anyCode && anyName)
                return await _projectsRepository.GetProjectById(id.Value);

            return await _projectsRepository.GetProject(id, codeSubStr, nameSubStr);
        }
    }
}
