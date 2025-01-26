using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectTasksTrackService.Core;
using ProjectTasksTrackService.Core.Repositories;
using ProjectTasksTrackService.Core.Results;
using ProjectTasksTrackService.Core.Services;

namespace ProjectTasksTrackService.BusinessLogic
{
    public class SubProjectsService : ISubProjectsService
    {
        private readonly ISubProjectsRepository _subProjectsRepository;

        public SubProjectsService(ISubProjectsRepository subProjectsRepository)
        {
            _subProjectsRepository = subProjectsRepository;
        }

        public Task<string> Create(ProjectSubDivision subproject)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteSubDivision(string projectId, int subDivisionId, string taskSecretString)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProjectSubDivision>> GetHotSubDivisions(
            string projectId = null,
            DateTime? deadLine = null,
            int skipCount = 0,
            int limitCount = 100)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectSubDivision> GetSubDivision(int subDivisionId, int? projectId = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProjectSubDivision>> GetSubDivisions(
            string projectId = null,
            int? intProjectId = null,
            int? subDivisionId = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100)
        {
            throw new NotImplementedException();
        }

        public async Task<ImportResult> Import(IEnumerable<ProjectSubDivision> subs)
        {
            if (subs is null)
                return new ImportResult(ErrorStrings.SUBPROJECTS_LIST_TO_IMPORT_SHOULD_NOT_BE_NULL, System.Net.HttpStatusCode.BadRequest);

            if (!subs.Any())
                return new ImportResult(ErrorStrings.SUBPROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED, System.Net.HttpStatusCode.BadRequest);

            var existingProjects = await _subProjectsRepository.GetAllProjectSubDivisions();

            List<int> conflictedIds = new List<int>();
            List<ProjectSubDivision> newSubsToImport = new List<ProjectSubDivision>();
            foreach (var impSub in subs)
            {
                if (existingProjects.Any(p => p.Id == impSub.Id))
                {
                    var existingProject = existingProjects.Single(p => p.Id == impSub.Id);
                    if (!impSub.Equals(existingProject))
                        conflictedIds.Add(impSub.Id);
                }
                else
                    newSubsToImport.Add(impSub);
            }

            if (conflictedIds.Any())
                return new ImportResult { ImportedCount = 0, Message = $"{ErrorStrings.SUBPROJECTS_CONFLICTS}:{string.Join(",", conflictedIds)}" };

            if (!newSubsToImport.Any())
                return new ImportResult { ImportedCount = 0, Message = ErrorStrings.ALREADY_IMPORTED };

            var importResult = await _subProjectsRepository.Import(newSubsToImport);

            return new ImportResult { ImportedCount = importResult.ImportedCount, Message = ErrorStrings.IMPORTED };
        }

        public Task<string> UpdateSubDivision(ProjectSubDivision subproject)
        {
            throw new NotImplementedException();
        }
    }
}
