using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectTasksTrackService.API.Contracts.Dto;
using ProjectTasksTrackService.API.Contracts.Dto.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ProjectTasksTrackService.API.Contracts.Interfaces
{
    /// <summary> Интерфейс контроллера управления задачами </summary>
    public interface ITasksAPI
    {
        /// <summary> Импорт задач (из старой системы) </summary>
        Task<IActionResult> Import(IEnumerable<OldTaskDto> projects);
        /// <summary> Создание задачи </summary>
        Task<IActionResult> Create(TaskDto task);

        /// <summary> Получение задачи </summary>
        Task<IActionResult> GetTask(int taskId, int? projectId = null, int? subdivisionId = null);

        /// <summary> Получение списка задач </summary>
        Task<IEnumerable<TaskDto>> GetTasks(
            int? projectId = null,
            int? subdivisionId = null,
            int? id = null,
            string codeSubStr = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100,
            bool ignoreCase = true);

        /// <summary> Получение списка задач (в старом компактном JSON-формате) для экспорта в старую систему </summary>
        Task<IEnumerable<OldTaskDto>> GetTasksOldDto(
            int? projectId = null,
            int? subdivisionId = null,
            int? id = null,
            string codeSubStr = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100,
            bool ignoreCase = true);

        /// <summary> Получение списка актуальных задач </summary>
        Task<IEnumerable<TaskDto>> GetHotTasks(
            int? projectId = null,
            int? subdivisionId = null,
            DateTime? deadLine = null,
            int skipCount = 0,
            int limitCount = 100);

        /// <summary> Обновление задачи </summary>
        Task<IActionResult> UpdateTask(TaskDto taskDto);
        
        /// <summary> Удаление задачи </summary>
        Task<IActionResult> DeleteTask(DeleteTaskRequestDto deleteTaskRequest);
    }
}
