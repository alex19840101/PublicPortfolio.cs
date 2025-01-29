using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectTasksTrackService.Core;
using ProjectTasksTrackService.Core.Repositories;
using ProjectTasksTrackService.Core.Results;
using Project = ProjectTasksTrackService.Core.Project;

namespace ProjectTasksTrackService.DataAccess.Repositories
{
    public class ProjectsRepository : IProjectsRepository
    {
        private readonly ProjectTasksTrackServiceDbContext _dbContext;

        public ProjectsRepository(ProjectTasksTrackServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreateResult> Add(Project project, bool trySetId = false)
        {
            ArgumentNullException.ThrowIfNull(project);

            var newProjectEntity = new Entities.Project(
                id: trySetId ? project.Id : await _dbContext.Projects.AsNoTracking().MaxAsync(p => p.Id) + 1,
                code: project.Code,
                name: project.Name,
                url: project.Url,
                imageUrl: project.ImageUrl,
                createdDt: project.CreatedDt == null ? DateTime.Now.ToUniversalTime() : project.CreatedDt.Value.ToUniversalTime(),
                lastUpdateDt: project.LastUpdateDt?.ToUniversalTime(),
                deadLineDt: project.DeadLineDt?.ToUniversalTime(),
                doneDt: project.DoneDt?.ToUniversalTime());

            await _dbContext.Projects.AddAsync(newProjectEntity);
            await _dbContext.SaveChangesAsync();
            
            await _dbContext.Entry(newProjectEntity).GetDatabaseValuesAsync(); //получение сгенерированного БД id
            return new CreateResult { Id = newProjectEntity.Id, StatusCode = HttpStatusCode.Created };
        }

        public async Task<ImportResult> Import(IEnumerable<Project> projects)
        {
            ArgumentNullException.ThrowIfNull(projects);
            if (!projects.Any())
                return new ImportResult { Message = ErrorStrings.PROJECTS_SHOULD_CONTAIN_AT_LEAST_1_PROJECT };

            IEnumerable<Entities.Project> projectEntities = projects.Select(p => new Entities.Project(
                id: p.Id,
                code: p.Code,
                name: p.Name,
                url: p.Url,
                imageUrl: p.ImageUrl,
                createdDt: p.CreatedDt == null ? DateTime.Now.ToUniversalTime() : p.CreatedDt.Value.ToUniversalTime(),
                lastUpdateDt: p.LastUpdateDt?.ToUniversalTime(),
                deadLineDt: p.DeadLineDt?.ToUniversalTime(),
                doneDt: p.DoneDt?.ToUniversalTime()));

            await _dbContext.Projects.AddRangeAsync(projectEntities);
            await _dbContext.SaveChangesAsync();

            return new ImportResult { ImportedCount = projects.Count(), Message = ErrorStrings.OK};
        }

        public async Task<Project> GetProjectById(int id)
        {
            var entityProject = await _dbContext.Projects
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == id);

            if (entityProject is null)
                return null;

            return Project(entityProject);
        }

        public async Task<Project> GetProject(
            int? id = null,
            string codeSubStr = null,
            string nameSubStr = null,
            bool ignoreCase = true)
        {
            var sc = ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.Ordinal;
            if (id is not null)
            {
                var entityProject = await _dbContext.Projects
                    .AsNoTracking()
                    .SingleOrDefaultAsync(p => p.Id == id);

                if (entityProject is null)
                    return null;

                if (!string.IsNullOrWhiteSpace(codeSubStr))
                    if (!entityProject.Code.Contains(codeSubStr, sc))
                        return null;

                if (!string.IsNullOrWhiteSpace(nameSubStr))
                    if (!entityProject.Name.Contains(nameSubStr, sc))
                        return null;

                return Project(entityProject);
            }
            //id == null

            List<Entities.Project> entityProjectsLst;
            Expression<Func<Entities.Project, bool>> expressionWhereCode = ignoreCase ?
                p => EF.Functions.Like(p.Code.ToLower(), $"%{codeSubStr.ToLower()}%") :
                p => p.Code.Contains(codeSubStr);
            Expression<Func<Entities.Project, bool>> expressionWhereName = ignoreCase ?
                p => EF.Functions.Like(p.Name.ToLower(), $"%{nameSubStr.ToLower()}%") :
                p => p.Name.Contains(nameSubStr);

            if (string.IsNullOrWhiteSpace(codeSubStr))
            {
                entityProjectsLst = await _dbContext.Projects
                        .AsNoTracking()
                        .Where(expressionWhereName).ToListAsync();

                if (entityProjectsLst.Count == 0)
                    return null;

                if (entityProjectsLst.Count > 1)
                    throw new InvalidOperationException(Core.ErrorStrings.MORE_THAN_ONE_PROJECT_FOUND);

                return Project(entityProjectsLst.Single());
            }

            //id == null, codeSubStr задан

            entityProjectsLst = string.IsNullOrWhiteSpace(nameSubStr) ?
                await _dbContext.Projects
                        .AsNoTracking()
                        .Where(expressionWhereCode).ToListAsync() :
                await _dbContext.Projects
                        .AsNoTracking()
                        .Where(expressionWhereCode)
                        .Where(expressionWhereName)
                        .ToListAsync();

            if (entityProjectsLst.Count == 0)
                return null;

            if (entityProjectsLst.Count > 1)
                throw new InvalidOperationException(Core.ErrorStrings.MORE_THAN_ONE_PROJECT_FOUND);

            return Project(entityProjectsLst.Single());
        }

        public async Task<IEnumerable<Project>> GetProjects(
            string codeSubStr = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100,
            bool ignoreCase = true)
        {
            limitCount = limitCount > 100 ? 100 : limitCount;
            List<Entities.Project> entityProjectsLst;

            if (string.IsNullOrWhiteSpace(codeSubStr) && string.IsNullOrWhiteSpace(nameSubStr))
            {
                entityProjectsLst = await _dbContext.Projects
                            .AsNoTracking()
                            .Skip(skipCount).Take(limitCount).ToListAsync();

                if (entityProjectsLst.Count == 0)
                    return [];

                return entityProjectsLst.Select(p => Project(p));
            }
            Expression<Func<Entities.Project, bool>> expressionWhereName = ignoreCase ?
                p => EF.Functions.Like(p.Name.ToLower(), $"%{nameSubStr.ToLower()}%") :
                p => p.Name.Contains(nameSubStr);

            if (string.IsNullOrWhiteSpace(codeSubStr))
            {
                entityProjectsLst = await _dbContext.Projects
                        .AsNoTracking()
                        .Where(expressionWhereName).Skip(skipCount).Take(limitCount).ToListAsync();

                if (entityProjectsLst.Count == 0)
                    return [];

                return entityProjectsLst.Select(p => Project(p));
            }

            //codeSubStr задан
            Expression<Func<Entities.Project, bool>> expressionWhereCode = ignoreCase ?
                p => EF.Functions.Like(p.Code.ToLower(), $"%{codeSubStr.ToLower()}%") :
                p => p.Code.Contains(codeSubStr);

            entityProjectsLst = string.IsNullOrWhiteSpace(nameSubStr) ?
                await _dbContext.Projects
                        .AsNoTracking()
                        .Where(expressionWhereCode).Skip(skipCount).Take(limitCount).ToListAsync() :
                await _dbContext.Projects
                        .AsNoTracking()
                        .Where(expressionWhereCode)
                        .Where(expressionWhereName).Skip(skipCount).Take(limitCount)
                        .ToListAsync();

            if (entityProjectsLst.Count == 0)
                return [];

            return entityProjectsLst.Select(p => Project(p));
        }

        public async Task<IEnumerable<Project>> GetAllProjects()
        {
            var projects = await _dbContext.Projects
                .AsNoTracking().Select(p => Project(p)).ToListAsync();

            return projects;
        }
        public async Task<UpdateResult> UpdateProject(Project project)
        {
            ArgumentNullException.ThrowIfNull(project);

            var entityProject = await _dbContext.Projects
                .SingleOrDefaultAsync(p => p.Id == project.Id);

            if (entityProject is null)
                return new UpdateResult(ErrorStrings.PROJECT_NOT_FOUND, HttpStatusCode.NotFound);

            if (!project.Code.Equals(entityProject.Code))
                return new UpdateResult(ErrorStrings.CODE_SHOULD_BE_THE_SAME, HttpStatusCode.Conflict);

            if (!string.Equals(project.Name, entityProject.Name))         entityProject.UpdateName(project.Name);
            if (!string.Equals(project.Url, entityProject.Url))           entityProject.UpdateUrl(project.Url);
            if (!string.Equals(project.ImageUrl, entityProject.ImageUrl)) entityProject.UpdateImageUrl(project.ImageUrl);

            if (project.CreatedDt?.ToUniversalTime() != entityProject.CreatedDt)
                entityProject.UpdateCreatedDt(project.CreatedDt?.ToUniversalTime());

            if (project.LastUpdateDt?.ToUniversalTime() != entityProject.LastUpdateDt)
                entityProject.UpdateLastUpdateDt(project.LastUpdateDt?.ToUniversalTime());

            if (project.DeadLineDt?.ToUniversalTime() != entityProject.DeadLineDt)
                entityProject.UpdateDeadLineDt(project.DeadLineDt?.ToUniversalTime());

            if (project.DoneDt?.ToUniversalTime() != entityProject.DoneDt)
                entityProject.UpdateDoneDt(project.DoneDt?.ToUniversalTime());


            if (_dbContext.ChangeTracker.HasChanges())
            {
                entityProject.UpdateLastUpdateDt(DateTime.Now.ToUniversalTime());
                await _dbContext.SaveChangesAsync();
                return new UpdateResult(ErrorStrings.PROJECT_UPDATED, HttpStatusCode.OK);
            }
            return new UpdateResult(ErrorStrings.PROJECT_IS_ACTUAL, HttpStatusCode.OK);
        }

        public async Task<string> DeleteProject(int id, string projectSecretString)
        {
            var entityProject = await _dbContext.Projects
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == id);

            if (entityProject is null)
                return Core.ErrorStrings.PROJECT_NOT_FOUND;
            
            if (string.IsNullOrWhiteSpace(projectSecretString))
                throw new ArgumentNullException(projectSecretString);

            _dbContext.Projects.Remove(entityProject);
            await _dbContext.SaveChangesAsync();

            return Core.ErrorStrings.OK;
        }
        private static Project Project(Entities.Project project) =>
            new Project(
            id: project.Id,
            code: project.Code,
            name: project.Name,
            url: project.Url,
            imageUrl: project.ImageUrl,
            createdDt: project.CreatedDt?.ToLocalTime(),
            lastUpdateDt: project.LastUpdateDt?.ToLocalTime(),
            deadLineDt: project.DeadLineDt?.ToLocalTime(),
            doneDt: project.DoneDt?.ToLocalTime());
    }
}
