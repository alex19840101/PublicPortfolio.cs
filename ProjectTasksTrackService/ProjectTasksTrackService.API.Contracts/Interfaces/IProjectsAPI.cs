using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
