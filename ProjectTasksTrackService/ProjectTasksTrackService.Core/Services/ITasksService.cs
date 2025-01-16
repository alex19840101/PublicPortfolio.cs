using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectTasksTrackService.Core.Services
{
    public interface ITasksService
    {
        Task<string> Import(IEnumerable<ProjectTask> tasks);
        Task<string> Create(ProjectTask project);
        Task<IEnumerable<ProjectTask>> GetAllTasks();
        Task<IEnumerable<ProjectTask>> GetAllHotTasks();
        Task<ProjectTask> GetTaskById(int taskId);
        Task<Project> GetProjectByNum(int legacyProjectNumber);
        Task<Project> GetProjectByName(string name);
        Task<string> UpdateTask(ProjectTask task);
        Task<string> UpdateName(string projectId, string newName);
        Task<string> UpdateUrl(string projectId, string url);
        Task<string> UpdateImageUrl(string projectId, string imageUrl);
        Task<string> UpdateScheduledDayNums(string projectId, HashSet<byte> scheduledDayNums);
        Task<string> DeleteTask(int taskId, string taskSecretString);
        Task<IEnumerable<ProjectTask>> GetTasksByTaskNameSubStr(string taskNameSubStr);
        Task<IEnumerable<ProjectTask>> GetTasks();
        Task<IEnumerable<ProjectTask>> GetAllHotTasksByDeadLine(DateTime deadLine);
    }
}
