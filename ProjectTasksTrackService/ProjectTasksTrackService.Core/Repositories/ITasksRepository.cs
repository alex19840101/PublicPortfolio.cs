﻿using System;
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
            int? projectId = null,
            int? subdivisionId = null,
            int? id = null,
            string codeSubStr = null,
            string nameSubStr = null,
            bool ignoreCase = true);
        Task<ProjectTask> GetTask(int taskId, int? projectId = null, int? subdivisionId = null);
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
        Task<string> DeleteTask(int taskId, string taskSecretString, int? projectId = null, int? subdivisionId = null);
    }
}
