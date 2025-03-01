using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectTasksTrackService.Core.Results;

namespace ProjectTasksTrackService.Core.Repositories
{
    public interface ISubProjectsRepository
    {
        Task<CreateResult> Add(ProjectSubDivision subDivision, bool trySetId = false);
        Task<ImportResult> Import(IEnumerable<ProjectSubDivision> subDivisions);
        Task<ProjectSubDivision> GetProjectSubDivision(
            int? projectId,
            int? id,
            string codeSubStr,
            string nameSubStr,
            bool ignoreCase);
        Task<ProjectSubDivision> GetProjectSubDivision(int subDivisionId, int? projectId);
        Task<IEnumerable<ProjectSubDivision>> GetProjectSubDivisions(
            int? projectId,
            int? id,
            string codeSubStr,
            string nameSubStr,
            int skipCount,
            int limitCount,
            bool ignoreCase);

        Task<IEnumerable<ProjectSubDivision>> GetAllProjectSubDivisions();

        Task<UpdateResult> UpdateSubDivision(ProjectSubDivision project);

        /// <summary>
        /// Удаление подпроекта
        /// </summary>
        /// <param name="id"> id подпроекта </param>
        /// <param name="projectSubDivionSecretString"> секретная строка для удаления подпроекта </param>
        /// <param name="projectId"> (опциональный) (сделан обязательным из-за CS0854) </param>
        /// <returns></returns>
        Task<DeleteResult> DeleteSubDivision(int id, string projectSubDivionSecretString, int? projectId);
        Task<IEnumerable<ProjectSubDivision>> GetHotSubDivisions(
            int? projectId,
            DateTime? deadLine,
            int skipCount,
            int limitCount);
    }
}
