using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectTasksTrackService.Core.Results;

namespace ProjectTasksTrackService.Core.Repositories
{
    public interface ISubProjectsRepository
    {
        Task<CreateResult> Add(ProjectSubDivision subDivision, bool trySetId = false);
        Task<ImportResult> Import(IEnumerable<ProjectSubDivision> subDivisions);
        Task<ProjectSubDivision> GetProjectSubDivision(
            int? projectId = null,
            int? id = null,
            string codeSubStr = null,
            string nameSubStr = null,
            bool ignoreCase = true);
        Task<ProjectSubDivision> GetProjectSubDivision(int subDivisionId, int? projectId = null);
        Task<IEnumerable<ProjectSubDivision>> GetProjectSubDivisions(
            int? projectId = null,
            int? id = null,
            string codeSubStr = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100,
            bool ignoreCase = true);

        Task<IEnumerable<ProjectSubDivision>> GetAllProjectSubDivisions();

        Task<UpdateResult> UpdateSubDivision(ProjectSubDivision project);
        Task<DeleteResult> DeleteSubDivision(int id, string projectSubDivionSecretString, int? projectId = null);
        Task<IEnumerable<ProjectSubDivision>> GetHotSubDivisions(
            int? projectId = null,
            DateTime? deadLine = null,
            int skipCount = 0,
            int limitCount = 100);
    }
}
