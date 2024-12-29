using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTasksTrackService.Contracts.Services
{
    public interface IProjectsService
    {
        Task<Project> Create(Project project);
    }
}
