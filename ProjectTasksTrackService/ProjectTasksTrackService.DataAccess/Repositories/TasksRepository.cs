using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ProjectTasksTrackService.Core;
using ProjectTasksTrackService.Core.Repositories;

namespace ProjectTasksTrackService.DataAccess.Repositories
{
    public class TasksRepository : ITasksRepository
    {
        public Task<string> Add(ProjectTask project)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteTask(string taskId, string taskSecretString)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateTask(ProjectTask project)
        {
            throw new NotImplementedException();
        }
    }
}
