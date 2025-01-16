﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectTasksTrackService.Core.Services
{
    public interface IProjectsService
    {
        Task<string> Import(IEnumerable<Project> projects);
        Task<string> Create(Project project);
        Task<IEnumerable<Project>> GetProjects(string projectId = null, int? intProjectId = null, string nameSubStr = null);
        Task<Project> GetProject(string projectId = null, int? intProjectId = null, string name = null);
        Task<string> UpdateProject(Project projectDto);
        Task<string> DeleteProject(string projectId, string projectSecretString);
    }
}
