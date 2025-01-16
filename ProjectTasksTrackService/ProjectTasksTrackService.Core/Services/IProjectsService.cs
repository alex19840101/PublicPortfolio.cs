using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectTasksTrackService.Core.Services
{
    public interface IProjectsService
    {
        Task<string> Import(IEnumerable<Project> projects);
        Task<string> Create(Project project);
        Task<IEnumerable<Project>> GetProjects();
        Task<Project> GetProjectById(string projectId);
        Task<Project> GetProjectByName(string name);
        Task<Project> GetProjectByNum(int intProjectId);
        Task<string> UpdateProject(Project projectDto);
        Task<string> DeleteProject(string projectId, string projectSecretString);
    }
}
