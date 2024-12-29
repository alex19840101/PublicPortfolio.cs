using System;
using System.Threading.Tasks;
using ProjectTasksTrackService.Contracts;
using ProjectTasksTrackService.Contracts.Services;

namespace ProjectTasksTrackService.BusinessLogic
{
    public class ProjectsService : IProjectsService
    {
        public Task<Project> Create(Project project)
        {
            throw new NotImplementedException();
        }
    }
}
