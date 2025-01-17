using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ProjectTasksTrackService.Core;
using ProjectTasksTrackService.Core.Repositories;
using ProjectTasksTrackService.Core.Services;

namespace ProjectTasksTrackService.BusinessLogic
{
    public class SubProjectsService : ISubProjectsService
    {
        private readonly ISubProjectsRepository _subProjectsRepository;

        public SubProjectsService(ISubProjectsRepository subProjectsRepository)
        {
            _subProjectsRepository = subProjectsRepository;
        }

        public Task<string> Create(ProjectSubDivision subproject)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteSubDivision(string projectId, int subDivisionId, string taskSecretString)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProjectSubDivision>> GetHotSubDivisions(
            string projectId = null,
            DateTime? deadLine = null,
            int skipCount = 0,
            int limitCount = 100)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectSubDivision> GetSubDivision(string projectId, int subDivisionId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProjectSubDivision>> GetSubDivisions(
            string projectId = null,
            int? intProjectId = null,
            int? subDivisionId = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100)
        {
            throw new NotImplementedException();
        }

        public Task<string> Import(IEnumerable<ProjectSubDivision> subprojects)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateSubDivision(ProjectSubDivision subproject)
        {
            throw new NotImplementedException();
        }
    }
}
