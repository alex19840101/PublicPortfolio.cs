using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectTasksTrackService.API.Contracts.Dto;
using ProjectTasksTrackService.API.Contracts.Dto.Requests;
using ProjectTasksTrackService.API.Contracts.Dto.Responses;
using ProjectTasksTrackService.API.Contracts.Interfaces;
using ProjectTasksTrackService.Core;
using ProjectTasksTrackService.Core.Results;
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
        [ProducesResponseType(typeof(ImportProjectsResponseDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ImportProjectsResponseDto), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Import(IEnumerable<OldProjectSubDivisionDto> oldSubDivisions)
        {
            List<ProjectSubDivision> subsList = [];
            foreach (var sub in oldSubDivisions)
            {
                subsList.Add(ProjectSubDivision(sub));
            }

            var importResult = await _subProjectsService.Import(subsList);

            if (importResult.StatusCode == HttpStatusCode.BadRequest)
                return new BadRequestObjectResult(new ProblemDetails { Title = importResult.Message });

            if (importResult.StatusCode == HttpStatusCode.Conflict || importResult.ImportedCount == 0)
                return new ConflictObjectResult(new ImportProjectsResponseDto
                {
                    Message = importResult.Message
                });

            return CreatedAtAction(nameof(Import), new ImportProjectsResponseDto
            {
                ImportedCount = importResult.ImportedCount,
                Message = importResult.Message
            });
        }

        /// <summary> Создание подпроекта </summary>
        [HttpPost("api/v2/SubDivisions/Create")]
        [ProducesResponseType(typeof(CreateProjectResponseDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(MessageResponseDto), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MessageResponseDto), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Create(ProjectSubDivisionDto subDivision)
        {
            var createResult = await _subProjectsService.Create(ProjectSubDivision(subDivision));

            if (createResult.StatusCode == HttpStatusCode.BadRequest)
                return new BadRequestObjectResult(new ProblemDetails { Title = createResult.Message });

            if (createResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new MessageResponseDto { Message = createResult.Message });

            if (createResult.StatusCode == HttpStatusCode.Conflict)
                return new ConflictObjectResult(new MessageResponseDto { Message = createResult.Message });

            return Ok(new CreateProjectResponseDto { Id = createResult.Id.Value, Code = subDivision.Code });
        }

        /// <summary> Получение списка подпроектов </summary>
        [HttpGet("api/v2/SubDivisions/GetSubDivisions")]
        public async Task<IEnumerable<ProjectSubDivisionDto>> GetSubDivisions(
            int? projectId = null,
            int? id = null,
            string codeSubStr = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100,
            bool ignoreCase = true)
        {
            var subDivisionsCollection = await _subProjectsService.GetSubDivisions(projectId, id, codeSubStr, nameSubStr, skipCount, limitCount, ignoreCase);
            List<ProjectSubDivisionDto> result = [];
            foreach (var subDivision in subDivisionsCollection)
            {
                result.Add(ProjectSubDivisionDto(subDivision));
            }

            return result;
        }

        /// <summary> Получение списка подпроектов (в старом компактном JSON-формате) для экспорта в старую систему </summary>
        [HttpGet("api/v2/SubDivisions/GetSubDivisionsOldDto")]
        [ProducesResponseType(typeof(OldProjectSubDivisionDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IEnumerable<OldProjectSubDivisionDto>> GetSubDivisionsOldDto(
            int? projectId = null,
            int? id = null,
            string codeSubStr = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100,
            bool ignoreCase = true)
        {
            var subDivisionsCollection = await _subProjectsService.GetSubDivisions(projectId, id, codeSubStr, nameSubStr, skipCount, limitCount, ignoreCase);
            List<OldProjectSubDivisionDto> result = [];
            foreach (var subDivision in subDivisionsCollection)
            {
                result.Add(OldProjectSubDivisionDto(subDivision));
            }

            return result;
        }

        /// <summary> Получение подпроекта </summary>
        [HttpGet("api/v2/SubDivisions/GetSubDivision")]
        [ProducesResponseType(typeof(ProjectSubDivisionDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(MessageResponseDto), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetSubDivision(int subDivisionId, int? projectId = null)
        {
            var subDivision = await _subProjectsService.GetSubDivision(subDivisionId, projectId);

            if (subDivision is null)
                return NotFound(new MessageResponseDto { Message = ErrorStrings.SUBDIVISION_NOT_FOUND });

            return Ok(ProjectSubDivisionDto(subDivision));
        }

        /// <summary> Получение списка актуальных подпроектов </summary>
        [HttpGet("api/v2/SubDivisions/GetHotSubDivisions")]
        [ProducesResponseType(typeof(ProjectSubDivisionDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
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
        [ProducesResponseType(typeof(UpdateResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(UpdateResult), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateSubDivision(ProjectSubDivisionDto subDivisionDto)
        {
            var updateResult = await _subProjectsService.UpdateSubDivision(ProjectSubDivision(subDivisionDto));

            if (updateResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(updateResult);

            if (updateResult.StatusCode == HttpStatusCode.Conflict)
                return new ConflictObjectResult(updateResult);

            return Ok(updateResult);
        }

        /// <summary> Удаление подпроекта </summary>
        [HttpDelete("api/v2/SubDivisions/DeleteSubDivision")]
        public async Task<string> DeleteSubDivision(DeleteProjectSubDivisionDto deleteSubProjectRequest)
        {
            return await _subProjectsService.DeleteSubDivision(
                deleteSubProjectRequest.SubDivisionId,
                deleteSubProjectRequest.SubDivisionSecretString,
                deleteSubProjectRequest.ProjectId);
        }

        #region Dto<->Core mappers
        [NonAction]
        private static ProjectSubDivision ProjectSubDivision(OldProjectSubDivisionDto oldDto) =>
            new ProjectSubDivision(
                id: oldDto.Id,
                projectId: oldDto.ProjectId,
                code: oldDto.Code,
                name: oldDto.Name,
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
                    code: dto.Code,
                    name: dto.Name,
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
                Code = dto.Code,
                Name = dto.Name,
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
                Code = dto.Code,
                Name = dto.Name,
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
