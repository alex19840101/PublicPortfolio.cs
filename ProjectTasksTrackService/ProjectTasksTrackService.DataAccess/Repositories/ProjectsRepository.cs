using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                imageUrl: project.ImageUrl);

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

        public Task<Project> GetProjectById(string projectId)
        {
            throw new NotImplementedException();
        }

        public Task<Project> GetProjectByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Project> GetProjectByNum(int intProjectId)
        {
            throw new NotImplementedException();
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
    }
}
