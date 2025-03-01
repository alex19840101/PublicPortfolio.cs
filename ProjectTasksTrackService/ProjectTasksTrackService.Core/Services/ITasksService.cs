using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectTasksTrackService.Core.Results;

namespace ProjectTasksTrackService.Core.Services
{
    public interface ITasksService
    {
        Task<ImportResult> Import(IEnumerable<ProjectTask> tasks);
        Task<CreateResult> Create(ProjectTask project);
        Task<ProjectTask> GetTask(int taskId, int? projectId, int? subdivisionId);
        Task<IEnumerable<ProjectTask>> GetHotTasks(
            int? projectId = null,
            int? subdivisionId = null,
            DateTime? deadLine = null,
            int skipCount = 0,
            int limitCount = 100);
        Task<IEnumerable<ProjectTask>> GetTasks(
            int? projectId = null,
            int? subdivisionId = null,
            int? id = null,
            string codeSubStr = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100,
            bool ignoreCase = true);

        Task<UpdateResult> UpdateTask(ProjectTask task);
        Task<DeleteResult> DeleteTask(int taskId, string taskSecretString, int? projectId = null, int? subdivisionId = null);
    }
}
