﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectTasksTrackService.Core;
using ProjectTasksTrackService.Core.Repositories;
using ProjectTasksTrackService.Core.Results;
using ProjectSubDivision = ProjectTasksTrackService.Core.ProjectSubDivision;

namespace ProjectTasksTrackService.DataAccess.Repositories
{
    public class SubProjectsRepository : ISubProjectsRepository
    {
        private readonly ProjectTasksTrackServiceDbContext _dbContext;

        public SubProjectsRepository(ProjectTasksTrackServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreateResult> Add(ProjectSubDivision sub, bool trySetId = false)
        {
            ArgumentNullException.ThrowIfNull(sub);

            var parentProject = await _dbContext.Projects.AsNoTracking().SingleOrDefaultAsync(p => p.Id == sub.ProjectId);

            if (parentProject is null)
                return new CreateResult
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = ErrorStrings.PARENT_PROJECT_NOT_FOUND
                };

            var newSubEntity = new Entities.ProjectSubDivision(
                id: trySetId ? sub.Id : await _dbContext.ProjectSubDivisions.AsNoTracking().MaxAsync(s => s.Id) + 1,
                projectId : sub.ProjectId,
                code: sub.Code,
                name: sub.Name,
                url1: sub.Url1,
                url2: sub.Url2,
                imageUrl: sub.ImageUrl,
                createdDt: sub.CreatedDt == null ? DateTime.Now.ToUniversalTime() : sub.CreatedDt.Value.ToUniversalTime(),
                lastUpdateDt: sub.LastUpdateDt?.ToUniversalTime(),
                deadLineDt: sub.DeadLineDt?.ToUniversalTime(),
                doneDt: sub.DoneDt?.ToUniversalTime());

            await _dbContext.ProjectSubDivisions.AddAsync(newSubEntity);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(newSubEntity).GetDatabaseValuesAsync(); //получение сгенерированного БД id
            return new CreateResult
            {
                Id = newSubEntity.Id,
                StatusCode = HttpStatusCode.Created,
                Code = newSubEntity.Code,
                SecretString = GetSecretString(newSubEntity)
            };
        }

        public async Task<ImportResult> Import(IEnumerable<ProjectSubDivision> subs)
        {
            ArgumentNullException.ThrowIfNull(subs);
            if (!subs.Any())
                return new ImportResult { Message = ErrorStrings.SUBDIVISIONS_SHOULD_CONTAIN_AT_LEAST_1_SUBDIVISION };

            IEnumerable<Entities.ProjectSubDivision> subdivisionEntities = subs.Select(s => new Entities.ProjectSubDivision(
                id: s.Id,
                projectId: s.ProjectId,
                code: s.Code,
                name: s.Name,
                url1: s.Url1,
                url2: s.Url2,
                imageUrl: s.ImageUrl,
                createdDt: s.CreatedDt == null ? DateTime.Now.ToUniversalTime() : s.CreatedDt.Value.ToUniversalTime(),
                lastUpdateDt: s.LastUpdateDt?.ToUniversalTime(),
                deadLineDt: s.DeadLineDt?.ToUniversalTime(),
                doneDt: s.DoneDt?.ToUniversalTime()));

            await _dbContext.ProjectSubDivisions.AddRangeAsync(subdivisionEntities);
            await _dbContext.SaveChangesAsync();

            return new ImportResult { ImportedCount = subs.Count(), Message = ErrorStrings.OK };
        }

        public async Task<ProjectSubDivision> GetProjectSubDivision(int subDivisionId, int? projectId = null)
        {
            var subDivisionEntity = await GetProjectSubDivisionEntity(subDivisionId, projectId);

            if (subDivisionEntity is null)
                return null;

            return ProjectSubDivision(subDivisionEntity);
        }

        private async Task<Entities.ProjectSubDivision> GetProjectSubDivisionEntity(int subDivisionId, int? projectId = null)
        {
            var subDivisionEntity = projectId != null ?
                    await _dbContext.ProjectSubDivisions.AsNoTracking().SingleOrDefaultAsync(s => s.Id == subDivisionId && s.ProjectId == projectId) :
                    await _dbContext.ProjectSubDivisions.AsNoTracking().SingleOrDefaultAsync(s => s.Id == subDivisionId);

            return subDivisionEntity;
        }

        public async Task<ProjectSubDivision> GetProjectSubDivision(
            int? projectId,
            int? id,
            string codeSubStr,
            string nameSubStr,
            bool ignoreCase)
        {
            if (string.IsNullOrWhiteSpace(codeSubStr) && string.IsNullOrWhiteSpace(nameSubStr))
                return await GetProjectSubDivision(id.Value, projectId);

            var sc = ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.Ordinal;

            var query = _dbContext.ProjectSubDivisions.AsNoTracking();

            if (id != null)
                query = query.Where(s => s.Id == id.Value);

            if (projectId != null)
                query = query.Where(s => s.ProjectId == projectId.Value);

            if (id is not null)
            {
                var subdivisionEntity = await query.SingleOrDefaultAsync();

                if (subdivisionEntity is null)
                    return null;

                if (!string.IsNullOrWhiteSpace(codeSubStr))
                    if (!subdivisionEntity.Code.Contains(codeSubStr, sc))
                        return null;

                if (!string.IsNullOrWhiteSpace(nameSubStr))
                    if (!subdivisionEntity.Name.Contains(nameSubStr, sc))
                        return null;

                return ProjectSubDivision(subdivisionEntity);
            }
            //id == null

            List<Entities.ProjectSubDivision> entityProjectSubDivisionsLst;
            Expression<Func<Entities.ProjectSubDivision, bool>> expressionWhereCode = ignoreCase ?
                s => EF.Functions.Like(s.Code.ToLower(), $"%{codeSubStr.ToLower()}%") :
                s => s.Code.Contains(codeSubStr);
            Expression<Func<Entities.ProjectSubDivision, bool>> expressionWhereName = ignoreCase ?
                s => EF.Functions.Like(s.Name.ToLower(), $"%{nameSubStr.ToLower()}%") :
                s => s.Name.Contains(nameSubStr);

            if (string.IsNullOrWhiteSpace(codeSubStr))
            {
                entityProjectSubDivisionsLst = await query.Where(expressionWhereName).ToListAsync();

                if (entityProjectSubDivisionsLst.Count == 0)
                    return null;

                if (entityProjectSubDivisionsLst.Count > 1)
                    throw new InvalidOperationException(Core.ErrorStrings.MORE_THAN_ONE_SUBDIVISION_FOUND);

                return ProjectSubDivision(entityProjectSubDivisionsLst.Single());
            }

            //id == null, codeSubStr задан

            entityProjectSubDivisionsLst = string.IsNullOrWhiteSpace(nameSubStr) ?
                await query
                        .Where(expressionWhereCode).ToListAsync() :
                await query
                        .Where(expressionWhereCode)
                        .Where(expressionWhereName)
                        .ToListAsync();

            if (entityProjectSubDivisionsLst.Count == 0)
                return null;

            if (entityProjectSubDivisionsLst.Count > 1)
                throw new InvalidOperationException(Core.ErrorStrings.MORE_THAN_ONE_SUBDIVISION_FOUND);

            return ProjectSubDivision(entityProjectSubDivisionsLst.Single());
        }

        public async Task<IEnumerable<ProjectSubDivision>> GetProjectSubDivisions(
            int? projectId = null,
            int? id = null,
            string codeSubStr = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100,
            bool ignoreCase = true)
        {
            limitCount = limitCount > 100 ? 100 : limitCount;
            List<Entities.ProjectSubDivision> entityProjectSubDivisionsLst;

            var query = _dbContext.ProjectSubDivisions.AsNoTracking();
            if (id != null)
                query = query.Where(s => s.Id == id.Value);
            if (projectId != null)
                query = query.Where(s => s.ProjectId == projectId.Value);

            if (string.IsNullOrWhiteSpace(codeSubStr) && string.IsNullOrWhiteSpace(nameSubStr))
            {
                entityProjectSubDivisionsLst = await query
                            .Skip(skipCount).Take(limitCount).ToListAsync();

                if (entityProjectSubDivisionsLst.Count == 0)
                    return [];

                return entityProjectSubDivisionsLst.Select(p => ProjectSubDivision(p));
            }
            Expression<Func<Entities.ProjectSubDivision, bool>> expressionWhereName = ignoreCase ?
                s => EF.Functions.Like(s.Name.ToLower(), $"%{nameSubStr.ToLower()}%") :
                s => s.Name.Contains(nameSubStr);

            if (string.IsNullOrWhiteSpace(codeSubStr))
            {
                entityProjectSubDivisionsLst = await query
                        .Where(expressionWhereName).Skip(skipCount).Take(limitCount).ToListAsync();

                if (entityProjectSubDivisionsLst.Count == 0)
                    return [];

                return entityProjectSubDivisionsLst.Select(p => ProjectSubDivision(p));
            }

            //codeSubStr задан
            Expression<Func<Entities.ProjectSubDivision, bool>> expressionWhereCode = ignoreCase ?
                s => EF.Functions.Like(s.Code.ToLower(), $"%{codeSubStr.ToLower()}%") :
                s => s.Code.Contains(codeSubStr);

            entityProjectSubDivisionsLst = string.IsNullOrWhiteSpace(nameSubStr) ?
                await query.Where(expressionWhereCode).Skip(skipCount).Take(limitCount).ToListAsync() :
                await query.Where(expressionWhereCode).Where(expressionWhereName).Skip(skipCount).Take(limitCount).ToListAsync();

            if (entityProjectSubDivisionsLst.Count == 0)
                return [];

            return entityProjectSubDivisionsLst.Select(s => ProjectSubDivision(s));
        }

        public async Task<IEnumerable<ProjectSubDivision>> GetAllProjectSubDivisions()
        {
            var subs = await _dbContext.ProjectSubDivisions
                .AsNoTracking().Select(s => ProjectSubDivision(s)).ToListAsync();

            return subs;
        }
        public async Task<UpdateResult> UpdateSubDivision(ProjectSubDivision sub)
        {
            ArgumentNullException.ThrowIfNull(sub);

            var entitySub = await _dbContext.ProjectSubDivisions
                .SingleOrDefaultAsync(s => s.Id == sub.Id);

            if (entitySub is null)
                return new UpdateResult(ErrorStrings.SUBDIVISION_NOT_FOUND, HttpStatusCode.NotFound);

            if (!sub.Code.Equals(entitySub.Code))
                return new UpdateResult(ErrorStrings.SUBDIVISION_CODE_SHOULD_BE_THE_SAME, HttpStatusCode.Conflict);

            if (!sub.ProjectId.Equals(entitySub.ProjectId))
                return new UpdateResult(ErrorStrings.SUBDIVISION_PROJECT_ID_SHOULD_BE_THE_SAME, HttpStatusCode.Conflict);

            if (!string.Equals(sub.Name, entitySub.Name)) entitySub.UpdateName(sub.Name);
            if (!string.Equals(sub.Url1, entitySub.Url1)) entitySub.UpdateUrl1(sub.Url1);
            if (!string.Equals(sub.Url2, entitySub.Url2)) entitySub.UpdateUrl2(sub.Url2);
            if (!string.Equals(sub.ImageUrl, entitySub.ImageUrl)) entitySub.UpdateImageUrl(sub.ImageUrl);

            if (sub.CreatedDt?.ToUniversalTime() != entitySub.CreatedDt)
                entitySub.UpdateCreatedDt(sub.CreatedDt?.ToUniversalTime());

            if (sub.LastUpdateDt?.ToUniversalTime() != entitySub.LastUpdateDt)
                entitySub.UpdateLastUpdateDt(sub.LastUpdateDt?.ToUniversalTime());

            if (sub.DeadLineDt?.ToUniversalTime() != entitySub.DeadLineDt)
                entitySub.UpdateDeadLineDt(sub.DeadLineDt?.ToUniversalTime());

            if (sub.DoneDt?.ToUniversalTime() != entitySub.DoneDt)
                entitySub.UpdateDoneDt(sub.DoneDt?.ToUniversalTime());

            if (_dbContext.ChangeTracker.HasChanges())
            {
                entitySub.UpdateLastUpdateDt(DateTime.Now.ToUniversalTime());
                await _dbContext.SaveChangesAsync();
                return new UpdateResult(ErrorStrings.SUBDIVISION_UPDATED, HttpStatusCode.OK);
            }
            return new UpdateResult(ErrorStrings.SUBDIVISION_IS_ACTUAL, HttpStatusCode.OK);
        }

        public async Task<DeleteResult> DeleteSubDivision(int id, string projectSubDivisionSecretString, int? projectId)
        {
            var subdivisionEntity = await GetProjectSubDivisionEntity(id, projectId);

            if (subdivisionEntity is null)
                return new DeleteResult(ErrorStrings.SUBDIVISION_NOT_FOUND, HttpStatusCode.NotFound);

            if (string.IsNullOrWhiteSpace(projectSubDivisionSecretString))
                return new DeleteResult(ErrorStrings.EMPTY_OR_NULL_SECRET_STRING, HttpStatusCode.Forbidden);

            if (!string.Equals(GetSecretString(subdivisionEntity), projectSubDivisionSecretString))
                return new DeleteResult(ErrorStrings.INVALID_SECRET_STRING, HttpStatusCode.Forbidden);

            _dbContext.ProjectSubDivisions.Remove(subdivisionEntity);
            await _dbContext.SaveChangesAsync();

            return new DeleteResult(ErrorStrings.OK, HttpStatusCode.OK);
        }

        private static ProjectSubDivision ProjectSubDivision(Entities.ProjectSubDivision sub) =>
            new ProjectSubDivision(
            id: sub.Id,
            projectId: sub.ProjectId,
            code: sub.Code,
            name: sub.Name,
            url1: sub.Url1,
            url2: sub.Url2,
            imageUrl: sub.ImageUrl,
            createdDt: sub.CreatedDt?.ToLocalTime(),
            lastUpdateDt: sub.LastUpdateDt?.ToLocalTime(),
            deadLineDt: sub.DeadLineDt?.ToLocalTime(),
            doneDt: sub.DoneDt?.ToLocalTime());

        public async Task<IEnumerable<ProjectSubDivision>> GetHotSubDivisions(
            int? projectId = null,
            DateTime? deadLine = null,
            int skipCount = 0,
            int limitCount = 100)
        {
            var query = _dbContext.ProjectSubDivisions.AsNoTracking().Where(s => s.DoneDt == null);
            if (projectId != null)
                query = query.Where(s => s.ProjectId == projectId.Value);

            if (deadLine != null)
                query = query.Where(s => s.DeadLineDt != null && s.DeadLineDt.Value.ToUniversalTime() <= deadLine.Value.ToUniversalTime()).OrderBy(s => s.DeadLineDt);

            var entityProjectSubDivisionsLst = await query.Skip(skipCount).Take(limitCount).ToListAsync();

            return entityProjectSubDivisionsLst.Select(s => ProjectSubDivision(s));
        }

        private static string GetSecretString(Entities.ProjectSubDivision sub)
        {
            return $"ProjectId={sub.ProjectId}.Id={sub.Id}".GetHashCode().ToString();
        }
    }
}
