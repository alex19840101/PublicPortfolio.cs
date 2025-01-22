using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectTasksTrackService.Core.Repositories
{
    public interface IProjectsRepository
    {
        Task<int> Add(Project project);
        Task<int> Import(IEnumerable<Project> projects);
        Task<Project> GetProject(int? id = null, string codeSubStr = null, string nameSubStr = null);
        Task<Project> GetProjectById(int id);
        Task<IEnumerable<Project>> GetProjects(
            string codeSubStr = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100);


        Task<string> UpdateProject(Project project);
        /*
        Task<string> UpdateName(string projectId, string newName);
        Task<string> UpdateUrl(string projectId, string url);
        Task<string> UpdateImageUrl(string projectId, string imageUrl);
        */
        Task<string> DeleteProject(int id, string projectSecretString);
    }
}
