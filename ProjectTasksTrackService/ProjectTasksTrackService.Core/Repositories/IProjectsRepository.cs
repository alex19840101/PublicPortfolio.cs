using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTasksTrackService.Core.Repositories
{
    public interface IProjectsRepository
    {
        Task<string> Add(Project project);
        Task<IEnumerable<Project>> GetProjects();
        Task<string> Import(IEnumerable<Project> projects);
    }
}
