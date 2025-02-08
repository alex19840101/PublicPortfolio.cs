using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectTasksTrackService.Core.Results;

namespace ProjectTasksTrackService.Core.Repositories
{
    public interface ITasksRepository
    {
        Task<ImportResult> Import(IEnumerable<ProjectTask> tasks);
        Task<CreateResult> Add(ProjectTask project, bool trySetId = false);
        Task<IEnumerable<ProjectTask>> GetAllTasks();
        Task<ProjectTask> GetTask(
            int? projectId,
            int? subdivisionId,
            int? id,
            string codeSubStr,
            string nameSubStr,
            bool ignoreCase);
        Task<ProjectTask> GetTask(int taskId, int? projectId, int? subdivisionId);
        Task<IEnumerable<ProjectTask>> GetHotTasks(
            int? projectId,
            int? subdivisionId,
            DateTime? deadLine,
            int skipCount,
            int limitCount);
        Task<IEnumerable<ProjectTask>> GetTasks(
            int? projectId,
            int? subdivisionId,
            int? id,
            string codeSubStr,
            string nameSubStr,
            int skipCount,
            int limitCount,
            bool ignoreCase);

        Task<UpdateResult> UpdateTask(ProjectTask task);
        Task<DeleteResult> DeleteTask(int taskId, string taskSecretString, int? projectId, int? subdivisionId);
    }
}
