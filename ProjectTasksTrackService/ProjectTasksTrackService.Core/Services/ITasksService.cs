using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectTasksTrackService.Core.Services
{
    public interface ITasksService
    {
        Task<string> Import(IEnumerable<ProjectTask> tasks);
        Task<string> Create(ProjectTask project);
        Task<ProjectTask> GetTask(string projectId, int taskId);
        Task<IEnumerable<ProjectTask>> GetHotTasks(
            DateTime? deadLine = null,
            int skipCount = 0,
            int limitCount = 100);
        Task<IEnumerable<ProjectTask>> GetTasks(
            string projectId = null,
            int? intProjectId = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100);
        Task<string> UpdateTask(ProjectTask task);
        Task<string> DeleteTask(int taskId, string taskSecretString);
    }
}
