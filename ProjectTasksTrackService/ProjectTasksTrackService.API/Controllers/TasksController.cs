using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectTasksTrackService.API.Contracts.Dto;
using ProjectTasksTrackService.API.Contracts.Dto.Requests;
using ProjectTasksTrackService.API.Contracts.Dto.Responses;
using ProjectTasksTrackService.API.Contracts.Interfaces;
using ProjectTasksTrackService.Core;
using ProjectTasksTrackService.Core.Enums;
using ProjectTasksTrackService.Core.Results;
using ProjectTasksTrackService.Core.Services;

namespace ProjectTasksTrackService.API.Controllers
{
    /// <summary> Контроллер управления проектами </summary>
    [ApiController]
    [Route("ProjectTasksTrackService")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class TasksController : ControllerBase, ITasksAPI
    {
        private readonly ITasksService _tasksService;
        /// <summary> </summary>
        public TasksController(ITasksService tasksService)
        {
            _tasksService = tasksService;
        }

        /// <summary> Импорт задач (из старой системы) </summary>
        [HttpPost("api/v2/Tasks/Import")]
        [ProducesResponseType(typeof(ImportResponseDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ImportResponseDto), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Import(IEnumerable<OldTaskDto> tasks)
        {
            List<ProjectTask> tasksCollection = [];
            foreach (var oldTaskDto in tasks)
            {
                tasksCollection.Add(ProjectTask(oldTaskDto));
            }

            var importResult = await _tasksService.Import(tasksCollection);

            if (importResult.StatusCode == HttpStatusCode.BadRequest)
                return new BadRequestObjectResult(new ProblemDetails { Title = importResult.Message });

            if (importResult.StatusCode == HttpStatusCode.Conflict || importResult.ImportedCount == 0)
                return new ConflictObjectResult(new ImportResponseDto
                {
                    Message = importResult.Message
                });

            return CreatedAtAction(nameof(Import), new ImportResponseDto
            {
                ImportedCount = importResult.ImportedCount,
                Message = importResult.Message
            });
        }

        /// <summary> Создание задачи </summary>
        [HttpPost("api/v2/Tasks/Create")]
        [ProducesResponseType(typeof(CreateResponseDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(MessageResponseDto), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MessageResponseDto), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Create(TaskDto taskDto)
        {
            var createResult = await _tasksService.Create(ProjectTask(taskDto));

            if (createResult.StatusCode == HttpStatusCode.BadRequest)
                return new BadRequestObjectResult(new ProblemDetails { Title = createResult.Message });

            if (createResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new MessageResponseDto { Message = createResult.Message });

            if (createResult.StatusCode == HttpStatusCode.Conflict)
                return new ConflictObjectResult(new MessageResponseDto { Message = createResult.Message });

            var result = new CreateResponseDto { Id = createResult.Id.Value, SecretString = createResult.SecretString };

            return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created};
        }

        /// <summary> Получение списка задач </summary>
        [HttpGet("api/v2/Tasks/GetTasks")]
        [ProducesResponseType(typeof(TaskDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IEnumerable<TaskDto>> GetTasks(
            int? projectId = null,
            int? subdivisionId = null,
            int? id = null,
            string codeSubStr = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100,
            bool ignoreCase = true)
        {
            var tasksCollection = await _tasksService.GetTasks(projectId, subdivisionId, id, codeSubStr, nameSubStr, skipCount, limitCount, ignoreCase);
            List<TaskDto> result = [];
            foreach (var task in tasksCollection)
            {
                result.Add(TaskDto(task));
            }

            return result;
        }

        /// <summary> Получение списка задач (в старом компактном JSON-формате) для экспорта в старую систему </summary>
        [HttpGet("api/v2/Tasks/GetTasksOldDto")]
        [ProducesResponseType(typeof(OldTaskDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IEnumerable<OldTaskDto>> GetTasksOldDto(
            int? projectId = null,
            int? subdivisionId = null,
            int? id = null,
            string codeSubStr = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100,
            bool ignoreCase = true)
        {
            var tasksCollection = await _tasksService.GetTasks(projectId, subdivisionId, id, codeSubStr, nameSubStr, skipCount, limitCount, ignoreCase);
            List<OldTaskDto> result = [];
            foreach (var task in tasksCollection)
            {
                result.Add(OldTaskDto(task));
            }

            return result;
        }

        /// <summary> Получение задачи </summary>
        [HttpGet("api/v2/Tasks/GetTask")]
        [ProducesResponseType(typeof(ProjectSubDivisionDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(MessageResponseDto), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetTask(int taskId, int? projectId = null, int? subdivisionId = null)
        {
            var task = await _tasksService.GetTask(taskId, projectId, subdivisionId);

            if (task is null)
                return NotFound(new MessageResponseDto { Message = ErrorStrings.TASK_NOT_FOUND });

            return Ok(TaskDto(task));
        }

        /// <summary> Получение списка актуальных задач </summary>
        [HttpGet("api/v2/Tasks/GetHotTasks")]
        [ProducesResponseType(typeof(TaskDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IEnumerable<TaskDto>> GetHotTasks(
            int? projectId = null,
            int? subdivisionId = null,
            DateTime? deadLine = null,
            int skipCount = 0,
            int limitCount = 100)
        {
            var tasksCollection = await _tasksService.GetHotTasks(projectId, subdivisionId, deadLine, skipCount, limitCount);
            List<TaskDto> result = [];
            foreach (var task in tasksCollection)
            {
                result.Add(TaskDto(task));
            }

            return result;
        }


        /// <summary> Обновление задачи </summary>
        [HttpPost("api/v2/Tasks/UpdateTask")]
        [ProducesResponseType(typeof(UpdateResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(UpdateResult), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateTask(TaskDto taskDto)
        {
            var updateResult = await _tasksService.UpdateTask(ProjectTask(taskDto));

            if (updateResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(updateResult);

            if (updateResult.StatusCode == HttpStatusCode.Conflict)
                return new ConflictObjectResult(updateResult);

            return Ok(updateResult);
        }

        /// <summary> Удаление задачи </summary>
        [HttpDelete("api/v2/Tasks/DeleteTask")]
        public async Task<string> DeleteTask(DeleteTaskRequestDto deleteProjectRequest)
        {
            return await _tasksService.DeleteTask(
                deleteProjectRequest.TaskId,
                deleteProjectRequest.TaskSecretString);
        }

        #region Dto<->Core mappers
        [NonAction]
        private static ProjectTask ProjectTask(OldTaskDto oldTaskDto) =>
            new ProjectTask(
                id: oldTaskDto.Id,
                projectId: oldTaskDto.ProjectId,
                code: oldTaskDto.Code,
                name: oldTaskDto.Name,
                repeatsType: (TaskRepeatsType)oldTaskDto.RepeatsType,
                repeatInDays: oldTaskDto.RepeatInDays,
                projectSubDivisionId: oldTaskDto.ProjectSubDivisionId,
                url1: oldTaskDto.Url1,
                url2: oldTaskDto.Url2,
                imageUrl: oldTaskDto.ImageUrl,

                deadLineDt: oldTaskDto.DeadLineDt,
                createdDt: oldTaskDto.CreatedDt,
                lastUpdateDt: oldTaskDto.LastUpdateDt,
                doneDt: oldTaskDto.DoneDateTime
            );

        [NonAction]
        private static ProjectTask ProjectTask(TaskDto taskDto) =>
            new ProjectTask(
                    id: taskDto.Id,
                    projectId: taskDto.ProjectId,
                    code: taskDto.Code,
                    name: taskDto.Name,
                    repeatsType: (TaskRepeatsType)taskDto.RepeatsType,
                    repeatInDays: taskDto.RepeatInDays,
                    projectSubDivisionId: taskDto.ProjectSubDivisionId,
                    url1: taskDto.Url1,
                    url2: taskDto.Url2,
                    imageUrl: taskDto.ImageUrl,

                    deadLineDt: taskDto.DeadLineDt,
                    createdDt: taskDto.CreatedDt,
                    lastUpdateDt: taskDto.LastUpdateDt,
                    doneDt: taskDto.DoneDateTime
            );

        [NonAction]
        private static TaskDto TaskDto(ProjectTask projectTask) =>
            new TaskDto
            {
                Id = projectTask.Id,
                ProjectId = projectTask.ProjectId,
                Code = projectTask.Code,
                Name = projectTask.Name,
                RepeatsType = (Contracts.Dto.Enums.TaskRepeatsType)projectTask.RepeatsType,
                RepeatInDays = projectTask.RepeatInDays,
                ProjectSubDivisionId = projectTask.ProjectSubDivisionId,
                Url1 = projectTask.Url1,
                Url2 = projectTask.Url2,
                ImageUrl = projectTask.ImageUrl,

                DeadLineDt = projectTask.DeadLineDt,
                CreatedDt = projectTask.CreatedDt,
                LastUpdateDt = projectTask.LastUpdateDt,
                DoneDateTime = projectTask.DoneDt
            };

        [NonAction]
        private static OldTaskDto OldTaskDto(ProjectTask projectTask) =>
            new OldTaskDto
            {
                Id = projectTask.Id,
                ProjectId = projectTask.ProjectId,
                Code = projectTask.Code,
                Name = projectTask.Name,
                RepeatsType = (Contracts.Dto.Enums.TaskRepeatsType)projectTask.RepeatsType,
                RepeatInDays = projectTask.RepeatInDays,
                ProjectSubDivisionId = projectTask.ProjectSubDivisionId,
                Url1 = projectTask.Url1,
                Url2 = projectTask.Url2,
                ImageUrl = projectTask.ImageUrl,

                DeadLineDt = projectTask.DeadLineDt,
                CreatedDt = projectTask.CreatedDt,
                LastUpdateDt = projectTask.LastUpdateDt,
                DoneDateTime = projectTask.DoneDt
            };

        #endregion Dto<->Core mappers
    }
}
