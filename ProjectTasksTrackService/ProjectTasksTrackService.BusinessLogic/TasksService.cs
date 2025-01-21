using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectTasksTrackService.Core;
using ProjectTasksTrackService.Core.Repositories;
using ProjectTasksTrackService.Core.Services;

namespace ProjectTasksTrackService.BusinessLogic
{
    public class TasksService : ITasksService
    {
        private readonly ITasksRepository _tasksRepository;

        public TasksService(ITasksRepository tasksRepository)
        {
            _tasksRepository = tasksRepository;
        }
        public Task<string> Import(IEnumerable<ProjectTask> tasks)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Create(ProjectTask projectTask)
        {
            if (string.IsNullOrWhiteSpace(projectTask.Name))
                throw new InvalidOperationException(ErrorStrings.TASK_NAME_SHOULD_NOT_BE_EMPTY);

            return await _tasksRepository.Add(projectTask);
        }

        public Task<IEnumerable<ProjectTask>> GetHotTasks(
            DateTime? deadLine = null,
            int skipCount = 0,
            int limitCount = 100)
        {
            throw new NotImplementedException();
        }
        public Task<ProjectTask> GetTask(string projectId, int taskId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProjectTask>> GetTasks(
            string projectId = null,
            int? intProjectId = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateTask(ProjectTask task)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteTask(int taskId, string taskSecretString)
        {
            throw new NotImplementedException();
        }
    }
}
