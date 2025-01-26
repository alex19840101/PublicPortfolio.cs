using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectTasksTrackService.Core.Services
{
    public interface ISubProjectsService
    {
        Task<string> Import(IEnumerable<ProjectSubDivision> subprojects);
        Task<string> Create(ProjectSubDivision subproject);
        Task<ProjectSubDivision> GetSubDivision(string projectId, int subDivisionId);
        Task<IEnumerable<ProjectSubDivision>> GetHotSubDivisions(
            string projectId = null,
            DateTime? deadLine = null,
            int skipCount = 0,
            int limitCount = 100);
        Task<IEnumerable<ProjectSubDivision>> GetSubDivisions(
            string projectId = null,
            int? intProjectId = null,
            int? subDivisionId = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100);
        Task<string> UpdateSubDivision(ProjectSubDivision subproject);
        Task<string> DeleteSubDivision(string projectId, int subDivisionId, string taskSecretString);
    }
}
