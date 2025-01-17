using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectTasksTrackService.API.Contracts.Dto;
using ProjectTasksTrackService.API.Contracts.Dto.Requests;

namespace ProjectTasksTrackService.API.Contracts.Interfaces
{
    /// <summary> Интерфейс контроллера управления задачами </summary>
    public interface ITasksAPI
    {
        /// <summary> Импорт задач (из старой системы) </summary>
        Task<string> Import(IEnumerable<OldTaskDto> projects);
        /// <summary> Создание задачи </summary>
        Task<string> Create(TaskDto task);

        /// <summary> Получение задачи </summary>
        Task<TaskDto> GetTask(string projectId, int taskId);

        /// <summary> Получение списка задач </summary>
        Task<IEnumerable<TaskDto>> GetTasks(
            string projectId = null,
            int? intProjectId = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100);

        /// <summary> Получение списка всех задач (в старом компактном JSON-формате) для экспорта в старую систему </summary>
        Task<IEnumerable<OldTaskDto>> GetTasksOldDto(
            string projectId = null,
            int? intProjectId = null,
            string nameSubStr = null,
            int skipCount = 0,
            int limitCount = 100);

        /// <summary> Получение списка всех актуальных задач </summary>
        Task<IEnumerable<TaskDto>> GetHotTasks(
            DateTime? deadLine = null,
            int skipCount = 0,
            int limitCount = 100);

        /// <summary> Обновление задачи </summary>
        Task<string> UpdateTask(TaskDto taskDto);
        
        /// <summary> Удаление задачи </summary>
        Task<string> DeleteTask(DeleteTaskRequestDto deleteTaskRequest);
    }
}
