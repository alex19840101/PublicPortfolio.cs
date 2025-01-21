using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectTasksTrackService.Core.Repositories;
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

        public async Task<int> Add(Project project)
        {
            ArgumentNullException.ThrowIfNull(project);

            var newProjectEntity = new Entities.Project(
                code: project.Code,
                name: project.Name,
                id: project.Id,
                url: project.Url,
                imageUrl: project.ImageUrl,
                createdDt: project.CreatedDt ?? DateTime.Now,
                lastUpdateDt: DateTime.Now);

            await _dbContext.Projects.AddAsync(newProjectEntity);
            await _dbContext.SaveChangesAsync();

            return newProjectEntity.Id;
        }

        public async Task<int> Import(IEnumerable<Project> projects)
        {
            ArgumentNullException.ThrowIfNull(projects);
            if (!projects.Any())
                throw new InvalidOperationException("projects should contain at least 1 project.");
            
            IEnumerable<Entities.Project> projectEntities = projects.Select(p => new Entities.Project(
                id: p.Id,
                code: p.Code,
                name: p.Name,
                url: p.Url,
                imageUrl: p.ImageUrl,
                createdDt: p.CreatedDt,
                lastUpdateDt: p.LastUpdateDt));

            await _dbContext.Projects.AddRangeAsync(projectEntities);
            await _dbContext.SaveChangesAsync();

            return projectEntities.Max(p => p.Id);
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

        public async Task<Project> GetProject(int? id = null, string codeSubStr = null, string nameSubStr = null)
        {
            if (id is not null)
            {
                var entityProject = await _dbContext.Projects
                    .AsNoTracking()
                    .SingleOrDefaultAsync(p => p.Id == id);

                if (entityProject is null)
                    return null;

                if (!string.IsNullOrWhiteSpace(codeSubStr))
                    if (!entityProject.Code.Contains(codeSubStr))
                        return null;

                if (!string.IsNullOrWhiteSpace(nameSubStr))
                    if (!entityProject.Name.Contains(nameSubStr))
                        return null;

                return Project(entityProject);
            }
            //id == null

            List<Entities.Project> entityProjectsLst;

            if (string.IsNullOrWhiteSpace(codeSubStr))
            {
                entityProjectsLst = await _dbContext.Projects
                        .AsNoTracking()
                        .Where(p => p.Name.Contains(nameSubStr)).ToListAsync();

                if (entityProjectsLst.Count == 0)
                    return null;

                if (entityProjectsLst.Count > 1)
                    throw new InvalidOperationException(Core.ErrorStrings.MORE_THAN_ONE_PROJECT_FOUND);
            }

            //id == null, codeSubStr задан

            entityProjectsLst = string.IsNullOrWhiteSpace(nameSubStr) ?
                await _dbContext.Projects
                        .AsNoTracking()
                        .Where(p => p.Code.Contains(codeSubStr)).ToListAsync() :
                await _dbContext.Projects
                        .AsNoTracking()
                        .Where(p => p.Code.Contains(codeSubStr))
                        .Where(p => p.Name.Contains(nameSubStr))
                        .ToListAsync();

            if (entityProjectsLst.Count == 0)
                return null;

            if (entityProjectsLst.Count > 1)
                throw new InvalidOperationException(Core.ErrorStrings.MORE_THAN_ONE_PROJECT_FOUND);

            return Project(entityProjectsLst.Single());
        }

        public Task<IEnumerable<Project>> GetProjects()
        {
            throw new NotImplementedException();
        }



        public Task<string> UpdateProject(Project project)
        {
            throw new NotImplementedException();
        }
        /*
        public Task<string> UpdateImageUrl(string projectId, string imageUrl)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateName(string projectId, string newName)
        {
            throw new NotImplementedException();
        }


        public Task<string> UpdateUrl(string projectId, string url)
        {
            throw new NotImplementedException();
        }

        */

        public Task<string> DeleteProject(string projectId, string projectSecretString)
        {
            throw new NotImplementedException();
        }

        private static Project Project(Entities.Project project) =>
            new Project(
            id: project.Id,
            code: project.Code,
            name: project.Name,
            url: project.Url,
            imageUrl: project.ImageUrl,
            createdDt: project.CreatedDt,
            lastUpdateDt: project.LastUpdateDt);
    }
}
