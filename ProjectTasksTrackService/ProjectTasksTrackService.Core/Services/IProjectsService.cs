using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectTasksTrackService.Core.Results;

namespace ProjectTasksTrackService.Core.Services
{
    public interface IProjectsService
    {
        Task<ImportResult> Import(IEnumerable<Project> projects);
        Task<CreateResult> Create(Project project);
        Task<IEnumerable<Project>> GetProjects(
            string codeSubStr = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100,
            bool ignoreCase = true);
        Task<Project> GetProject(
            int? id,
            string code = null,
            string name = null,
            bool ignoreCase = true);
        Task<UpdateResult> UpdateProject(Project projectDto);
        Task<DeleteResult> DeleteProject(int id, string projectSecretString);
    }
}
