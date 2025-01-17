using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectTasksTrackService.API.Contracts.Dto;
using ProjectTasksTrackService.API.Contracts.Dto.Requests;
using ProjectTasksTrackService.API.Contracts.Interfaces;
using ProjectTasksTrackService.Core;
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
        public async Task<string> Import(IEnumerable<OldTaskDto> tasks)
        {
            List<ProjectTask> tasksCollection = [];
            foreach (var oldTaskDto in tasks)
            {
                tasksCollection.Add(ProjectTask(oldTaskDto));
            }

            return await _tasksService.Import(tasksCollection);
        }

        /// <summary> Создание задачи </summary>
        [HttpPost("api/v2/Tasks/Create")]
        public async Task<string> Create(TaskDto taskDto)
        {
            return await _tasksService.Create(ProjectTask(taskDto));
        }

        /// <summary> Получение списка задач </summary>
        [HttpGet("api/v2/Tasks/GetTasks")]
        public async Task<IEnumerable<TaskDto>> GetTasks(
            string projectId = null,
            int? intProjectId = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100)
        {
            var tasksCollection = await _tasksService.GetTasks(projectId,intProjectId, nameSubStr, skipCount, limitCount);
            List<TaskDto> result = [];
            foreach (var task in tasksCollection)
            {
                result.Add(TaskDto(task));
            }

            return result;
        }

        /// <summary> Получение списка задач (в старом компактном JSON-формате) для экспорта в старую систему </summary>
        [HttpGet("api/v2/Tasks/GetTasksOldDto")]
        public async Task<IEnumerable<OldTaskDto>> GetTasksOldDto(
            string projectId = null,
            int? intProjectId = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100)
        {
            var tasksCollection = await _tasksService.GetTasks(projectId, intProjectId, nameSubStr, skipCount, limitCount);
            List<OldTaskDto> result = [];
            foreach (var task in tasksCollection)
            {
                result.Add(OldTaskDto(task));
            }

            return result;
        }

        /// <summary> Получение задачи </summary>
        [HttpGet("api/v2/Tasks/GetTask")]
        public async Task<TaskDto> GetTask(string projectId, int taskId)
        {
            var task = await _tasksService.GetTask(projectId, taskId);
            return TaskDto(task);
        }

        /// <summary> Получение списка актуальных задач </summary>
        [HttpGet("api/v2/Tasks/GetHotTasks")]
        public async Task<IEnumerable<TaskDto>> GetHotTasks(
            DateTime? deadLine = null,
            int skipCount = 0,
            int limitCount = 100)
        {
            var tasksCollection = await _tasksService.GetHotTasks(deadLine, skipCount, limitCount);
            List<TaskDto> result = [];
            foreach (var task in tasksCollection)
            {
                result.Add(TaskDto(task));
            }

            return result;
        }


        /// <summary> Обновление задачи </summary>
        [HttpPost("api/v2/Tasks/UpdateTask")]
        public async Task<string> UpdateTask(TaskDto taskDto)
        {
            return await _tasksService.UpdateTask(ProjectTask(taskDto));
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
                projectId: oldTaskDto.ProjectId,
                name: oldTaskDto.Name,
                intProjectId: oldTaskDto.IntProjectId,
                projectSubDivisionId: oldTaskDto.ProjectSubDivisionId,
                url1: oldTaskDto.Url1,
                url2: oldTaskDto.Url2,
                imageUrl: oldTaskDto.ImageUrl,
                deadLineDt: oldTaskDto.DeadLineDt,
                createdDt: oldTaskDto.CreatedDt,
                lastUpdateDt: oldTaskDto.LastUpdateDt
            );

        [NonAction]
        private static ProjectTask ProjectTask(TaskDto taskDto) =>
            new ProjectTask(
                    projectId: taskDto.ProjectId,
                    name: taskDto.Name,
                    intProjectId: taskDto.IntProjectId,
                    projectSubDivisionId: taskDto.ProjectSubDivisionId,
                    url1: taskDto.Url1,
                    url2: taskDto.Url2,
                    imageUrl: taskDto.ImageUrl,
                    deadLineDt: taskDto.DeadLineDt,
                    createdDt: taskDto.CreatedDt,
                    lastUpdateDt: taskDto.LastUpdateDt
            );

        [NonAction]
        private static TaskDto TaskDto(ProjectTask projectTask) =>
            new TaskDto
            {
                ProjectId = projectTask.ProjectId,
                Name = projectTask.Name,
                IntProjectId = projectTask.IntProjectId,
                ProjectSubDivisionId = projectTask.ProjectSubDivisionId,
                Url1 = projectTask.Url1,
                Url2 = projectTask.Url2,
                ImageUrl = projectTask.ImageUrl,
                DeadLineDt = projectTask.DeadLineDt,
                CreatedDt = projectTask.CreatedDt,
                LastUpdateDt = projectTask.LastUpdateDt
            };

        [NonAction]
        private static OldTaskDto OldTaskDto(ProjectTask projectTask) =>
            new OldTaskDto
            {
                ProjectId = projectTask.ProjectId,
                Name = projectTask.Name,
                IntProjectId = projectTask.IntProjectId,
                ProjectSubDivisionId = projectTask.ProjectSubDivisionId,
                Url1 = projectTask.Url1,
                Url2 = projectTask.Url2,
                ImageUrl = projectTask.ImageUrl,
                DeadLineDt = projectTask.DeadLineDt,
                CreatedDt = projectTask.CreatedDt,
                LastUpdateDt = projectTask.LastUpdateDt
            };

        #endregion Dto<->Core mappers
    }
}
