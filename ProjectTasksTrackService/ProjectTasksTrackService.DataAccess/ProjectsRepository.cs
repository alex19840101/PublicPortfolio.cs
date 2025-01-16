using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectTasksTrackService.Core;
using ProjectTasksTrackService.Core.Repositories;

namespace ProjectTasksTrackService.DataAccess
{
    public class ProjectsRepository : IProjectsRepository
    {
        public Task<string> Add(Project project)
        {
            throw new NotImplementedException();
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

        public Task<string> Import(IEnumerable<Project> projects)
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

        public Task<string> UpdateScheduledDayNums(string projectId, HashSet<byte> scheduledDayNums)
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
