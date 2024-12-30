using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectTasksTrackService.Core;
using ProjectTasksTrackService.Core.Repositories;

namespace ProjectTasksTrackService.DataAccess
{
    public class ProjectsRepository : IProjectsRepository
    {
        public Task<string> Add(Project project)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Project>> GetProjects()
        {
            throw new NotImplementedException();
        }

        public Task<string> Import(IEnumerable<Project> projects)
        {
            throw new NotImplementedException();
        }
    }
}
