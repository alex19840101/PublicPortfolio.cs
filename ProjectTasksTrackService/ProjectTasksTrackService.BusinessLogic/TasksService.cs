using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectTasksTrackService.Core;
using ProjectTasksTrackService.Core.Repositories;
using ProjectTasksTrackService.Core.Results;
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
        public async Task<ImportResult> Import(IEnumerable<ProjectTask> tasks)
        {
            if (tasks is null)
                return new ImportResult(ErrorStrings.TASKS_LIST_TO_IMPORT_SHOULD_NOT_BE_NULL, System.Net.HttpStatusCode.BadRequest);

            if (!tasks.Any())
                return new ImportResult(ErrorStrings.TASKS_LIST_TO_IMPORT_SHOULD_BE_FILLED, System.Net.HttpStatusCode.BadRequest);

            var existingTasks = await _tasksRepository.GetAllTasks();

            List<int> conflictedIds = new List<int>();
            List<ProjectTask> newTasksToImport = new List<ProjectTask>();
            foreach (var impTask in tasks)
            {
                if (existingTasks.Any(t => t.Id == impTask.Id))
                {
                    var existingTask = existingTasks.Single(t => t.Id == impTask.Id);
                    if (!impTask.Equals(existingTask))
                        conflictedIds.Add(impTask.Id);
                }
                else
                    newTasksToImport.Add(impTask);
            }

            if (conflictedIds.Any())
                return new ImportResult { ImportedCount = 0, Message = $"{ErrorStrings.TASKS_CONFLICTS}:{string.Join(",", conflictedIds)}" };

            if (!newTasksToImport.Any())
                return new ImportResult { ImportedCount = 0, Message = ErrorStrings.ALREADY_IMPORTED };

            var importResult = await _tasksRepository.Import(newTasksToImport);

            return new ImportResult { ImportedCount = importResult.ImportedCount, Message = ErrorStrings.IMPORTED };
        }

        public async Task<CreateResult> Create(ProjectTask task)
        {
            if (!string.IsNullOrWhiteSpace(task.Code))
                return new CreateResult(ErrorStrings.TASK_CODE_SHOULD_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(task.Name))
                return new CreateResult(ErrorStrings.TASK_NAME_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (task.Id != 0)
                return new CreateResult(ErrorStrings.TASK_ID_SHOULD_BE_ZERO, System.Net.HttpStatusCode.BadRequest);

            var createResult = await _tasksRepository.Add(task);

            return createResult;
        }

        public async Task<IEnumerable<ProjectTask>> GetHotTasks(
            int? projectId = null,
            int? subdivisionId = null,
            DateTime? deadLine = null,
            int skipCount = 0,
            int limitCount = 100)
        {
            var tasks = await _tasksRepository.GetHotTasks(
                projectId: projectId,
                subdivisionId: subdivisionId,
                deadLine: deadLine,
                skipCount: skipCount,
                limitCount: limitCount);

            return tasks;
        }
        public async Task<ProjectTask> GetTask(int taskId, int? projectId = null, int? subdivisionId = null)
        {
            return await _tasksRepository.GetTask(taskId, projectId, subdivisionId);
        }

        public async Task<IEnumerable<ProjectTask>> GetTasks(
            int? projectId = null,
            int? subdivisionId = null,
            int? id = null,
            string codeSubStr = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100,
            bool ignoreCase = true)
        {
            return await _tasksRepository.GetTasks(
                projectId: projectId,
                subdivisionId: subdivisionId,
                id: id,
                codeSubStr: codeSubStr,
                nameSubStr: nameSubStr,
                skipCount: skipCount,
                limitCount: limitCount,
                ignoreCase: ignoreCase);
        }

        public async Task<UpdateResult> UpdateTask(ProjectTask task)
        {
            if (string.IsNullOrWhiteSpace(task.Code))
                return new UpdateResult
                {
                    Message = ErrorStrings.TASK_CODE_SHOULD_BE_THE_SAME,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

            if (string.IsNullOrWhiteSpace(task.Name))
                return new UpdateResult
                {
                    Message = ErrorStrings.TASK_NAME_SHOULD_NOT_BE_EMPTY,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

            return await _tasksRepository.UpdateTask(task);
        }

        public async Task<DeleteResult> DeleteTask(
            int taskId,
            string taskSecretString,
            int? projectId = null,
            int? subdivisionId = null)
        {
            return await _tasksRepository.DeleteTask(taskId, taskSecretString, projectId, subdivisionId);
        }
    }
}
