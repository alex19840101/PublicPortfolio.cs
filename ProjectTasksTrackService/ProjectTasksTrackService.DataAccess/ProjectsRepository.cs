using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectTasksTrackService.Contracts;
using ProjectTasksTrackService.Contracts.Repositories;

namespace ProjectTasksTrackService.DataAccess
{
    public class ProjectsRepository : IProjectsRepository
    {
        public Task<Project> Add(Project project)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Project>> GetProjects()
        {
            throw new NotImplementedException();
        }
    }
}
