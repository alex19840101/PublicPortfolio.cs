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
    public class SubProjectsService : ISubProjectsService
    {
        private readonly ISubProjectsRepository _subProjectsRepository;

        public SubProjectsService(ISubProjectsRepository subProjectsRepository)
        {
            _subProjectsRepository = subProjectsRepository;
        }

        public async Task<CreateResult> Create(ProjectSubDivision subproject)
        {
            if (subproject == null)
                throw new ArgumentNullException(ErrorStrings.SUBPROJECT_PARAM_NAME);

            if (string.IsNullOrWhiteSpace(subproject.Code))
                return new CreateResult(ErrorStrings.SUBPROJECT_CODE_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(subproject.Name))
                return new CreateResult(ErrorStrings.SUBPROJECT_NAME_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (subproject.Id != 0)
                return new CreateResult(ErrorStrings.SUBPROJECT_ID_SHOULD_BE_ZERO, System.Net.HttpStatusCode.BadRequest);

            var createResult = await _subProjectsRepository.Add(subproject);

            return createResult;
        }

        public async Task<DeleteResult> DeleteSubDivision(int subDivisionId, string projectSubDivionSecretString, int? projectId = null)
        {
            return await _subProjectsRepository.DeleteSubDivision(subDivisionId, projectSubDivionSecretString, projectId);
        }

        public async Task<IEnumerable<ProjectSubDivision>> GetHotSubDivisions(
            int? projectId = null,
            DateTime? deadLine = null,
            int skipCount = 0,
            int limitCount = 100)
        {
            var subdivisions = await _subProjectsRepository.GetHotSubDivisions(projectId, deadLine, skipCount, limitCount);
            return subdivisions;
        }

        public async Task<ProjectSubDivision> GetSubDivision(int subDivisionId, int? projectId = null)
        {
            return await _subProjectsRepository.GetProjectSubDivision(subDivisionId, projectId);
        }

        public async Task<IEnumerable<ProjectSubDivision>> GetSubDivisions(
            int? projectId = null,
            int? id = null,
            string codeSubStr = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100,
            bool ignoreCase = true)
        {
            return await _subProjectsRepository.GetProjectSubDivisions(projectId, id, codeSubStr, nameSubStr, skipCount, limitCount, ignoreCase);
        }

        public async Task<ImportResult> Import(IEnumerable<ProjectSubDivision> subs)
        {
            if (subs is null)
                return new ImportResult(ErrorStrings.SUBPROJECTS_LIST_TO_IMPORT_SHOULD_NOT_BE_NULL, System.Net.HttpStatusCode.BadRequest);

            if (!subs.Any())
                return new ImportResult(ErrorStrings.SUBPROJECTS_LIST_TO_IMPORT_SHOULD_BE_FILLED, System.Net.HttpStatusCode.BadRequest);

            var existingSubs = await _subProjectsRepository.GetAllProjectSubDivisions();

            List<int> conflictedIds = new List<int>();
            List<ProjectSubDivision> newSubsToImport = new List<ProjectSubDivision>();
            foreach (var impSub in subs)
            {
                if (existingSubs.Any(s => s.Id == impSub.Id))
                {
                    var existingSub = existingSubs.Single(s => s.Id == impSub.Id);
                    if (!impSub.Equals(existingSub))
                        conflictedIds.Add(impSub.Id);
                }
                else
                    newSubsToImport.Add(impSub);
            }

            if (conflictedIds.Any())
                return new ImportResult
                {
                    ImportedCount = 0,
                    Message = $"{ErrorStrings.SUBPROJECTS_CONFLICTS}:{string.Join(",", conflictedIds)}",
                    StatusCode = System.Net.HttpStatusCode.Conflict
                };

            if (!newSubsToImport.Any())
                return new ImportResult
                {
                    ImportedCount = 0,
                    Message = ErrorStrings.ALREADY_IMPORTED,
                    StatusCode = System.Net.HttpStatusCode.OK
                };

            var importResult = await _subProjectsRepository.Import(newSubsToImport);

            return new ImportResult 
            {
                ImportedCount = importResult.ImportedCount,
                Message = ErrorStrings.IMPORTED,
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<UpdateResult> UpdateSubDivision(ProjectSubDivision subproject)
        {
            if (string.IsNullOrWhiteSpace(subproject.Code))
                return new UpdateResult
                {
                    Message = ErrorStrings.SUBPROJECT_CODE_SHOULD_NOT_BE_EMPTY,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

            if (string.IsNullOrWhiteSpace(subproject.Name))
                return new UpdateResult
                {
                    Message = ErrorStrings.SUBPROJECT_NAME_SHOULD_NOT_BE_EMPTY,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

            return await _subProjectsRepository.UpdateSubDivision(subproject);
        }
    }
}
