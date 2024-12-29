using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTasksTrackService.Contracts.Repositories
{
    public interface IProjectsRepository
    {
        Task<Project> Add(Project project);
        Task<IEnumerable<Project>> GetProjects();
    }
}
