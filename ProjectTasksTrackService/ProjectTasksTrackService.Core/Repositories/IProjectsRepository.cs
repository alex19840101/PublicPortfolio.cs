using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectTasksTrackService.Core.Repositories
{
    public interface IProjectsRepository
    {
        Task<string> Add(Project project);
        Task<IEnumerable<Project>> GetProjects();
        Task<Project> GetProjectById(string projectId);
        Task<Project> GetProjectByNum(int legacyProjectNumber);
        Task<Project> GetProjectByName(string name);
        Task<string> Import(IEnumerable<Project> projects);
        Task<string> UpdateProject(Project projectDto);
        Task<string> UpdateName(string projectId, string newName);
        Task<string> UpdateUrl(string projectId, string url);
        Task<string> UpdateImageUrl(string projectId, string imageUrl);
        Task<string> UpdateScheduledDayNums(string projectId, HashSet<byte> scheduledDayNums);
        Task<string> DeleteProject(string projectId, string projectSecretString);
    }
}
