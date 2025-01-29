using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectTasksTrackService.Core;
using ProjectTasksTrackService.Core.Enums;
using ProjectTasksTrackService.Core.Repositories;
using ProjectTasksTrackService.Core.Results;
using ProjectTask = ProjectTasksTrackService.Core.ProjectTask;

namespace ProjectTasksTrackService.DataAccess.Repositories
{
    public class TasksRepository : ITasksRepository
    {
         
        private readonly ProjectTasksTrackServiceDbContext _dbContext;

        public TasksRepository(ProjectTasksTrackServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreateResult> Add(ProjectTask task, bool trySetId = false)
        {
            ArgumentNullException.ThrowIfNull(task);

            var parentProject = await _dbContext.Projects.AsNoTracking().SingleOrDefaultAsync(p => p.Id == task.ProjectId);

            if (parentProject is null)
                return new CreateResult
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = ErrorStrings.PARENT_PROJECT_NOT_FOUND
                };
            var subCode = string.Empty;
            if (task.ProjectSubDivisionId is not null)
            {
                var parentSub = await _dbContext.ProjectSubDivisions.AsNoTracking().SingleOrDefaultAsync(s => s.Id == task.ProjectSubDivisionId.Value);

                if (parentSub is null)
                    return new CreateResult
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = ErrorStrings.PARENT_SUBDIVISION_NOT_FOUND
                    };
                subCode = $".{parentSub.Code}";
            }
            int id = trySetId ? task.Id : await _dbContext.ProjectTasks.AsNoTracking().MaxAsync(p => p.Id) + 1;

            var code = $"{parentProject.Code}{subCode}.{id}";

            var newTaskEntity = new Entities.ProjectTask(
                id: id,
                projectId : task.ProjectId,
                code: code,
                name: task.Name,
                repeatsType: (short)task.RepeatsType,
                repeatInDays: task.RepeatInDays,
                projectSubDivisionId: task.ProjectSubDivisionId,
                url1: task.Url1,
                url2: task.Url2,
                imageUrl: task.ImageUrl,

                createdDt: task.CreatedDt == null ? DateTime.Now.ToUniversalTime() : task.CreatedDt.Value.ToUniversalTime(),
                lastUpdateDt: task.LastUpdateDt == null ? null : task.LastUpdateDt.Value.ToUniversalTime(),
                deadLineDt: task.DeadLineDt == null ? null : task.DeadLineDt.Value.ToUniversalTime(),
                doneDt: task.DoneDt == null ? null : task.DoneDt.Value.ToUniversalTime());

            await _dbContext.ProjectTasks.AddAsync(newTaskEntity);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(newTaskEntity).GetDatabaseValuesAsync(); //получение сгенерированного БД id
            return new CreateResult { Id = newTaskEntity.Id, StatusCode = HttpStatusCode.Created };
        }
        

        public async Task<ImportResult> Import(IEnumerable<ProjectTask> tasks)
        {
            ArgumentNullException.ThrowIfNull(tasks);
            if (!tasks.Any())
                return new ImportResult { Message = ErrorStrings.TASKS_SHOULD_CONTAIN_AT_LEAST_1_TASK };

            IEnumerable<Entities.ProjectTask> tasksEntities = tasks.Select(task => new Entities.ProjectTask(
                id: task.Id,
                projectId : task.ProjectId,
                code: task.Code,
                name: task.Name,
                repeatsType: (short)task.RepeatsType,
                repeatInDays: task.RepeatInDays,
                projectSubDivisionId: task.ProjectSubDivisionId,
                url1: task.Url1,
                url2: task.Url2,
                imageUrl: task.ImageUrl,

                createdDt: task.CreatedDt == null ? DateTime.Now.ToUniversalTime() : task.CreatedDt.Value.ToUniversalTime(),
                lastUpdateDt: task.LastUpdateDt == null ? null : task.LastUpdateDt.Value.ToUniversalTime(),
                deadLineDt: task.DeadLineDt == null ? null : task.DeadLineDt.Value.ToUniversalTime(),
                doneDt: task.DoneDt == null ? null : task.DoneDt.Value.ToUniversalTime()));

            await _dbContext.ProjectTasks.AddRangeAsync(tasksEntities);
            await _dbContext.SaveChangesAsync();

            return new ImportResult { ImportedCount = tasks.Count(), Message = ErrorStrings.OK };
        }

        public async Task<IEnumerable<ProjectTask>> GetAllTasks()
        {
            var tasks = await _dbContext.ProjectTasks
                .AsNoTracking().Select(t => ProjectTask(t)).ToListAsync();

            return tasks;
        }

        public async Task<ProjectTask> GetTask(int taskId, int? projectId = null, int? subdivisionId = null)
        {
            var taskEntity = await GetTaskEntity(taskId, projectId, subdivisionId);
            if (taskEntity is null)
                return null;

            return ProjectTask(taskEntity);
        }

        private async Task<Entities.ProjectTask> GetTaskEntity(int taskId, int? projectId = null, int? subdivisionId = null)
        {
            var query = _dbContext.ProjectTasks.AsNoTracking().Where(t => t.Id == taskId);

            if (subdivisionId != null)
                query = query.Where(t => t.ProjectSubDivisionId == subdivisionId.Value);

            if (projectId != null)
                query = query.Where(t => t.ProjectId == projectId.Value);

            var taskEntity = await query.SingleOrDefaultAsync();

            return taskEntity;
        }

        public async Task<ProjectTask> GetTask(
            int? projectId = null,
            int? subdivisionId = null,
            int? id = null,
            string codeSubStr = null,
            string nameSubStr = null,
            bool ignoreCase = true)
        {
            if (string.IsNullOrWhiteSpace(codeSubStr) && string.IsNullOrWhiteSpace(nameSubStr))
                return await GetTask(id.Value, projectId, subdivisionId);

            var sc = ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.Ordinal;

            var query = _dbContext.ProjectTasks.AsNoTracking();

            if (id != null)
                query = query.Where(t => t.Id == id.Value);

            if (subdivisionId != null)
                query = query.Where(t => t.ProjectSubDivisionId == subdivisionId.Value);
            if (projectId != null)
                query = query.Where(t => t.ProjectId == projectId.Value);

            if (id is not null)
            {
                var taskEntity = await query.SingleOrDefaultAsync();

                if (taskEntity is null)
                    return null;

                if (!string.IsNullOrWhiteSpace(codeSubStr))
                    if (!taskEntity.Code.Contains(codeSubStr, sc))
                        return null;

                if (!string.IsNullOrWhiteSpace(nameSubStr))
                    if (!taskEntity.Name.Contains(nameSubStr, sc))
                        return null;

                return ProjectTask(taskEntity);
            }
            //id == null

            List<Entities.ProjectTask> entityProjectTasksLst;
            Expression<Func<Entities.ProjectTask, bool>> expressionWhereCode = ignoreCase ?
                t => EF.Functions.Like(t.Code.ToLower(), $"%{codeSubStr.ToLower()}%") :
                t => t.Code.Contains(codeSubStr);
            Expression<Func<Entities.ProjectTask, bool>> expressionWhereName = ignoreCase ?
                t => EF.Functions.Like(t.Name.ToLower(), $"%{nameSubStr.ToLower()}%") :
                t => t.Name.Contains(nameSubStr);

            if (string.IsNullOrWhiteSpace(codeSubStr))
            {
                entityProjectTasksLst = await query.Where(expressionWhereName).ToListAsync();

                if (entityProjectTasksLst.Count == 0)
                    return null;

                if (entityProjectTasksLst.Count > 1)
                    throw new InvalidOperationException(Core.ErrorStrings.MORE_THAN_ONE_SUBDIVISION_FOUND);

                return ProjectTask(entityProjectTasksLst.Single());
            }

            //id == null, codeSubStr задан

            entityProjectTasksLst = string.IsNullOrWhiteSpace(nameSubStr) ?
                await query
                        .Where(expressionWhereCode).ToListAsync() :
                await query
                        .Where(expressionWhereCode)
                        .Where(expressionWhereName)
                        .ToListAsync();

            if (entityProjectTasksLst.Count == 0)
                return null;

            if (entityProjectTasksLst.Count > 1)
                throw new InvalidOperationException(Core.ErrorStrings.MORE_THAN_ONE_SUBDIVISION_FOUND);

            return ProjectTask(entityProjectTasksLst.Single());
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
            limitCount = limitCount > 100 ? 100 : limitCount;
            List<Entities.ProjectTask> entityProjectTasksLst;

            var query = _dbContext.ProjectTasks.AsNoTracking();

            if (id != null)
                query = query.Where(t => t.Id == id.Value);

            if (subdivisionId != null)
                query = query.Where(t => t.ProjectSubDivisionId == subdivisionId.Value);

            if (projectId != null)
                query = query.Where(t => t.ProjectId == projectId.Value);

            if (string.IsNullOrWhiteSpace(codeSubStr) && string.IsNullOrWhiteSpace(nameSubStr))
            {
                entityProjectTasksLst = await query
                            .Skip(skipCount).Take(limitCount).ToListAsync();

                if (entityProjectTasksLst.Count == 0)
                    return [];

                return entityProjectTasksLst.Select(p => ProjectTask(p));
            }
            Expression<Func<Entities.ProjectTask, bool>> expressionWhereName = ignoreCase ?
                t => EF.Functions.Like(t.Name.ToLower(), $"%{nameSubStr.ToLower()}%") :
                t => t.Name.Contains(nameSubStr);

            if (string.IsNullOrWhiteSpace(codeSubStr))
            {
                entityProjectTasksLst = await query
                        .Where(expressionWhereName).Skip(skipCount).Take(limitCount).ToListAsync();

                if (entityProjectTasksLst.Count == 0)
                    return [];

                return entityProjectTasksLst.Select(p => ProjectTask(p));
            }

            //codeSubStr задан
            Expression<Func<Entities.ProjectTask, bool>> expressionWhereCode = ignoreCase ?
                t => EF.Functions.Like(t.Code.ToLower(), $"%{codeSubStr.ToLower()}%") :
                t => t.Code.Contains(codeSubStr);

            entityProjectTasksLst = string.IsNullOrWhiteSpace(nameSubStr) ?
                await query.Where(expressionWhereCode).Skip(skipCount).Take(limitCount).ToListAsync() :
                await query.Where(expressionWhereCode).Where(expressionWhereName).Skip(skipCount).Take(limitCount).ToListAsync();

            if (entityProjectTasksLst.Count == 0)
                return [];

            return entityProjectTasksLst.Select(t => ProjectTask(t));
        }

        

        public async Task<UpdateResult> UpdateTask(ProjectTask task)
        {
            ArgumentNullException.ThrowIfNull(task);

            var entityTask = await _dbContext.ProjectTasks
                .SingleOrDefaultAsync(s => s.Id == task.Id);

            if (entityTask is null)
                return new UpdateResult(ErrorStrings.SUBDIVISION_NOT_FOUND, HttpStatusCode.NotFound);

            if (!task.Code.Equals(entityTask.Code))
                return new UpdateResult(ErrorStrings.CODE_SHOULD_BE_THE_SAME, HttpStatusCode.Conflict);

            if (!task.ProjectId.Equals(entityTask.ProjectId))
                return new UpdateResult(ErrorStrings.TASK_PROJECT_ID_SHOULD_BE_THE_SAME, HttpStatusCode.Conflict);

            if (!task.ProjectSubDivisionId.Equals(entityTask.ProjectSubDivisionId))
                return new UpdateResult(ErrorStrings.TASK_PROJECTSUBDIVISIONID_SHOULD_BE_THE_SAME, HttpStatusCode.Conflict);

            if (!string.Equals(task.Name, entityTask.Name)) entityTask.UpdateName(task.Name);
            if (!string.Equals(task.Url1, entityTask.Url1)) entityTask.UpdateUrl1(task.Url1);
            if (!string.Equals(task.Url2, entityTask.Url2)) entityTask.UpdateUrl2(task.Url2);
            if (!string.Equals(task.ImageUrl, entityTask.ImageUrl)) entityTask.UpdateImageUrl(task.ImageUrl);

            if (task.CreatedDt != null && entityTask.CreatedDt == null)
                entityTask.UpdateCreatedDt(task.CreatedDt.Value.ToUniversalTime());

            if (task.LastUpdateDt != null && entityTask.LastUpdateDt == null)
                entityTask.UpdateLastUpdateDt(task.LastUpdateDt.Value.ToUniversalTime());

            if (task.DeadLineDt != null && entityTask.DeadLineDt == null)
                entityTask.UpdateDeadLineDt(task.DeadLineDt.Value.ToUniversalTime());

            if (task.DoneDt != null && entityTask.DoneDt == null)
                entityTask.UpdateDoneDt(task.DoneDt.Value.ToUniversalTime());

            if (_dbContext.ChangeTracker.HasChanges())
            {
                entityTask.UpdateLastUpdateDt(DateTime.Now.ToUniversalTime());
                await _dbContext.SaveChangesAsync();
                return new UpdateResult(ErrorStrings.TASK_UPDATED, HttpStatusCode.OK);
            }
            return new UpdateResult(ErrorStrings.TASK_IS_ACTUAL, HttpStatusCode.OK);
        }

        
        public async Task<string> DeleteTask(int id, string projectSubDivisionSecretString, int? projectId, int? subdivisionId)
        {
            var taskEntity = await GetTaskEntity(id, projectId, subdivisionId);

            if (taskEntity is null)
                return Core.ErrorStrings.TASK_NOT_FOUND;

            if (string.IsNullOrWhiteSpace(projectSubDivisionSecretString))
                throw new ArgumentNullException(projectSubDivisionSecretString);

            _dbContext.ProjectTasks.Remove(taskEntity);
            await _dbContext.SaveChangesAsync();

            return Core.ErrorStrings.OK;
        }
        private static ProjectTask ProjectTask(Entities.ProjectTask sub) =>
            new ProjectTask(
            id: sub.Id,
            projectId: sub.ProjectId,
            code: sub.Code,
            name: sub.Name,
            repeatsType: (TaskRepeatsType)sub.RepeatsType,
            repeatInDays: sub.RepeatInDays,
            projectSubDivisionId: sub.ProjectSubDivisionId,
            url1: sub.Url1,
            url2: sub.Url2,
            imageUrl: sub.ImageUrl,
            createdDt: sub.CreatedDt?.ToLocalTime(),
            lastUpdateDt: sub.LastUpdateDt?.ToLocalTime(),
            deadLineDt: sub.DeadLineDt?.ToLocalTime(),
            doneDt: sub.DoneDt?.ToLocalTime());
        public async Task<IEnumerable<ProjectTask>> GetHotTasks(
            int? projectId = null,
            int? subdivisionId = null,
            DateTime? deadLine = null,
            int skipCount = 0,
            int limitCount = 100)
        {
            var query = _dbContext.ProjectTasks.AsNoTracking().Where(s => s.DoneDt == null);
            if (subdivisionId != null)
                query = query.Where(t => t.ProjectSubDivisionId == subdivisionId.Value);

            if (projectId != null)
                query = query.Where(t => t.ProjectId == projectId.Value);

            if (deadLine != null)
                query = query.Where(t => t.DeadLineDt != null && t.DeadLineDt.Value.ToUniversalTime() <= deadLine.Value.ToUniversalTime()).OrderBy(s => s.DeadLineDt);

            var entityProjectTasksLst = await query.Skip(skipCount).Take(limitCount).ToListAsync();

            return entityProjectTasksLst.Select(t => ProjectTask(t));
        }

    }
}
