using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTasksTrackService.Core.Services
{
    public interface ISubProjectsService
    {
        Task<string> Import(IEnumerable<ProjectSubDivision> subprojects);
        Task<string> Create(ProjectSubDivision subproject);
        Task<string> UpdateSubDivision(ProjectSubDivision subproject);
        Task<string> DeleteSubDivision(string projectId, int subDivisionId, string taskSecretString);
    }
}
