using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectTasksTrackService.API.Contracts.Dto;
using ProjectTasksTrackService.API.Contracts.Dto.Requests;
using ProjectTasksTrackService.API.Contracts.Interfaces;
using ProjectTasksTrackService.BusinessLogic;
using ProjectTasksTrackService.Core;
using ProjectTasksTrackService.Core.Services;

namespace ProjectTasksTrackService.API.Controllers
{
    /// <summary> Контроллер управления подпроектами </summary>
    [ApiController]
    [Route("ProjectTasksTrackService")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class ProjectSubDivisionsController : ControllerBase, IProjectSubDivisionAPI
    {
        private readonly ISubProjectsService _subProjectsService;
        /// <summary> </summary>
        public ProjectSubDivisionsController(ISubProjectsService subProjectsService)
        {
            _subProjectsService = subProjectsService;
        }

        /// <summary> Импорт подпроектов (из старой системы) </summary>
        [HttpPost("api/v2/SubDivisions/Import")]
        public async Task<string> Import(IEnumerable<OldProjectSubDivisionDto> oldSubDivisions)
        {
            List<ProjectSubDivision> subDivisionsCollection = [];
            foreach (var oldSubDivisionDto in oldSubDivisions)
            {
                subDivisionsCollection.Add(ProjectSubDivision(oldSubDivisionDto));
            }

            return await _subProjectsService.Import(subDivisionsCollection);
        }

        /// <summary> Создание подпроекта </summary>
        [HttpPost("api/v2/SubDivisions/Create")]
        public async Task<string> Create(ProjectSubDivisionDto subDivision)
        {
            return await _subProjectsService.Create(ProjectSubDivision(subDivision));
        }

        /// <summary> Получение списка подпроектов </summary>
        [HttpGet("api/v2/SubDivisions/GetSubDivisions")]
        public async Task<IEnumerable<ProjectSubDivisionDto>> GetSubDivisions(
            string projectId = null,
            int? intProjectId = null,
            int? subDivisionId = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100)
        {
            var subDivisionsCollection = await _subProjectsService.GetSubDivisions(projectId, intProjectId, subDivisionId, nameSubStr, skipCount, limitCount);
            List<ProjectSubDivisionDto> result = [];
            foreach (var subDivision in subDivisionsCollection)
            {
                result.Add(ProjectSubDivisionDto(subDivision));
            }

            return result;
        }

        /// <summary> Получение списка подпроектов (в старом компактном JSON-формате) для экспорта в старую систему </summary>
        [HttpGet("api/v2/SubDivisions/GetSubDivisionsOldDto")]
        public async Task<IEnumerable<OldProjectSubDivisionDto>> GetSubDivisionsOldDto(
            string projectId = null,
            int? intProjectId = null,
            int? subDivisionId = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100)
        {
            var subDivisionsCollection = await _subProjectsService.GetSubDivisions(projectId, intProjectId, subDivisionId, nameSubStr, skipCount, limitCount);
            List<OldProjectSubDivisionDto> result = [];
            foreach (var subDivision in subDivisionsCollection)
            {
                result.Add(OldProjectSubDivisionDto(subDivision));
            }

            return result;
        }

        /// <summary> Получение подпроекта </summary>
        [HttpGet("api/v2/SubDivisions/GetSubDivision")]
        public async Task<ProjectSubDivisionDto> GetSubDivision(string projectId, int subDivisionId)
        {
            var subDivision = await _subProjectsService.GetSubDivision(projectId, subDivisionId);
            return ProjectSubDivisionDto(subDivision);
        }

        /// <summary> Получение списка актуальных подпроектов </summary>
        [HttpGet("api/v2/SubDivisions/GetHotSubDivisions")]
        public async Task<IEnumerable<ProjectSubDivisionDto>> GetHotSubDivisions(
            string projectId = null,
            DateTime? deadLine = null,
            int skipCount = 0,
            int limitCount = 100)
        {
            var subDivisionsCollection = await _subProjectsService.GetHotSubDivisions(projectId, deadLine, skipCount, limitCount);
            List<ProjectSubDivisionDto> result = [];
            foreach (var subDivision in subDivisionsCollection)
            {
                result.Add(ProjectSubDivisionDto(subDivision));
            }

            return result;
        }

        /// <summary> Обновление подпроекта </summary>
        [HttpPost("api/v2/SubDivisions/UpdateSubDivision")]
        public async Task<string> UpdateSubDivision(ProjectSubDivisionDto subDivisionDto)
        {
            return await _subProjectsService.UpdateSubDivision(ProjectSubDivision(subDivisionDto));
        }

        /// <summary> Удаление подпроекта </summary>
        [HttpDelete("api/v2/SubDivisions/DeleteSubDivision")]
        public async Task<string> DeleteSubDivision(DeleteProjectSubDivisionDto deleteSubProjectRequest)
        {
            return await _subProjectsService.DeleteSubDivision(
                deleteSubProjectRequest.ProjectId,
                deleteSubProjectRequest.SubDivisionId,
                deleteSubProjectRequest.SubDivisionSecretString);
        }

        #region Dto<->Core mappers
        [NonAction]
        private static ProjectSubDivision ProjectSubDivision(OldProjectSubDivisionDto oldDto) =>
            new ProjectSubDivision(
                id: oldDto.Id,
                projectId: oldDto.ProjectId,
                name: oldDto.Name,
                intProjectId: oldDto.IntProjectId,
                url1: oldDto.Url1,
                url2: oldDto.Url2,
                imageUrl: oldDto.ImageUrl,
                deadLineDt: oldDto.DeadLineDt,
                createdDt: oldDto.CreatedDt,
                lastUpdateDt: oldDto.LastUpdateDt
            );

        [NonAction]
        private static ProjectSubDivision ProjectSubDivision(ProjectSubDivisionDto dto) =>
            new ProjectSubDivision(
                    id: dto.Id,
                    projectId: dto.ProjectId,
                    name: dto.Name,
                    intProjectId: dto.IntProjectId,
                    url1: dto.Url1,
                    url2: dto.Url2,
                    imageUrl: dto.ImageUrl,
                    deadLineDt: dto.DeadLineDt,
                    createdDt: dto.CreatedDt,
                    lastUpdateDt: dto.LastUpdateDt
            );

        [NonAction]
        private static ProjectSubDivisionDto ProjectSubDivisionDto(ProjectSubDivision dto) =>
            new ProjectSubDivisionDto
            {
                Id = dto.Id,
                ProjectId = dto.ProjectId,
                Name = dto.Name,
                IntProjectId = dto.IntProjectId,
                Url1 = dto.Url1,
                Url2 = dto.Url2,
                ImageUrl = dto.ImageUrl,
                DeadLineDt = dto.DeadLineDt,
                CreatedDt = dto.CreatedDt,
                LastUpdateDt = dto.LastUpdateDt
            };

        [NonAction]
        private static OldProjectSubDivisionDto OldProjectSubDivisionDto(ProjectSubDivision dto) =>
            new OldProjectSubDivisionDto
            {
                Id = dto.Id,
                ProjectId = dto.ProjectId,
                Name = dto.Name,
                IntProjectId = dto.IntProjectId,
                Url1 = dto.Url1,
                Url2 = dto.Url2,
                ImageUrl = dto.ImageUrl,
                DeadLineDt = dto.DeadLineDt,
                CreatedDt = dto.CreatedDt,
                LastUpdateDt = dto.LastUpdateDt
            };

        #endregion Dto<->Core mappers
    }
}
