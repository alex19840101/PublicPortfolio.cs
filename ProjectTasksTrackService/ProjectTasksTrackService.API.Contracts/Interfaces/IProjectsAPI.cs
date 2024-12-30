using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectTasksTrackService.API.Contracts.Dto;

namespace ProjectTasksTrackService.API.Contracts.Interfaces
{
    public interface IProjectsAPI
    {
        Task<string> Create(ProjectDto project);
        Task<IEnumerable<ProjectDto>> GetProjects();
        Task<ProjectDto> GetProjectById(string projectId);
        Task<ProjectDto> GetProjectByNum(int legacyProjectNumber);
        Task<ProjectDto> GetProjectByName(string name);
        Task<string> Import(IEnumerable<OldProjectDto> projects);
        Task<string> UpdateProject(ProjectDto projectDto);
        Task<string> UpdateName(string projectId, string newName);
        Task<string> UpdateUrl(string projectId, string url);
        Task<string> UpdateImageUrl(string projectId, string imageUrl);
        Task<string> UpdateScheduledDayNums(string projectId, HashSet<byte> scheduledDayNums);
        Task<string> DeleteProject(string projectId, string projectSecretString);
    }
}
