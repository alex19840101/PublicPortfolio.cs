﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectTasksTrackService.Core.Services
{
    public interface IProjectsService
    {
        Task<int> Import(IEnumerable<Project> projects);
        Task<int> Create(Project project);
        Task<IEnumerable<Project>> GetProjects(
            string projectId = null,
            int? intProjectId = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100);
        Task<Project> GetProject(string projectId = null, int? intProjectId = null, string name = null);
        Task<string> UpdateProject(Project projectDto);
        Task<string> DeleteProject(int id, string projectSecretString);
    }
}
