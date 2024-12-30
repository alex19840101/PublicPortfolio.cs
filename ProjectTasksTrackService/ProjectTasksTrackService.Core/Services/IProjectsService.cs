using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTasksTrackService.Core.Services
{
    public interface IProjectsService
    {
        Task<string> Create(Project project);
        Task<IEnumerable<Project>> GetProjects();
        Task<string> Import(IEnumerable<Project> projects);
    }
}
