using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectTasksTrackService.Core.Results;

namespace ProjectTasksTrackService.Core.Services
{
    public interface ISubProjectsService
    {
        Task<ImportResult> Import(IEnumerable<ProjectSubDivision> subprojects);
        Task<CreateResult> Create(ProjectSubDivision subproject);
        Task<ProjectSubDivision> GetSubDivision(int subDivisionId, int? projectId = null);
        Task<IEnumerable<ProjectSubDivision>> GetHotSubDivisions(
            int? projectId = null,
            DateTime? deadLine = null,
            int skipCount = 0,
            int limitCount = 100);
        Task<IEnumerable<ProjectSubDivision>> GetSubDivisions(
            int? projectId = null,
            int? id = null,
            string codeSubStr = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100,
            bool ignoreCase = true);
        Task<UpdateResult> UpdateSubDivision(ProjectSubDivision subproject);
        Task<DeleteResult> DeleteSubDivision(int subDivisionId, string subDivisionSecretString, int? projectId = null);
    }
}
