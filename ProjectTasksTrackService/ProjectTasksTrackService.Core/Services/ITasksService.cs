using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectTasksTrackService.Core.Services
{
    public interface ITasksService
    {
        Task<string> Import(IEnumerable<ProjectTask> tasks);
        Task<string> Create(ProjectTask project);
        Task<IEnumerable<ProjectTask>> GetAllHotTasks();
        Task<IEnumerable<ProjectTask>> GetAllHotTasksByDeadLine(DateTime deadLine);
        Task<IEnumerable<ProjectTask>> GetAllTasks();
        Task<ProjectTask> GetTaskById(int taskId);
        Task<IEnumerable<ProjectTask>> GetTasksByTaskNameSubStr(string taskNameSubStr);
        Task<IEnumerable<ProjectTask>> GetTasksForProject(string projectId, string taskNameSubStr);
        Task<string> UpdateTask(ProjectTask task);
        Task<string> DeleteTask(int taskId, string taskSecretString);
    }
}
