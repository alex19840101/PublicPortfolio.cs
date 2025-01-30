using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectTasksTrackService.Core.Results;

namespace ProjectTasksTrackService.Core.Repositories
{
    public interface IProjectsRepository
    {
        Task<CreateResult> Add(Project project, bool trySetId = false);
        Task<ImportResult> Import(IEnumerable<Project> projects);
        Task<Project> GetProject(
            int? id = null,
            string codeSubStr = null,
            string nameSubStr = null,
            bool ignoreCase = true);
        Task<Project> GetProjectById(int id);
        Task<IEnumerable<Project>> GetProjects(
            string codeSubStr = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100,
            bool ignoreCase = true);

        Task<IEnumerable<Project>> GetAllProjects();

        Task<UpdateResult> UpdateProject(Project project);
        Task<DeleteResult> DeleteProject(int id, string projectSecretString);
    }
}
