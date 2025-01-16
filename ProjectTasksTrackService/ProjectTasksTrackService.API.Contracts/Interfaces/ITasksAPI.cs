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

        /// <summary> Получение списка всех задач (в старом компактном JSON-формате) для экспорта в старую систему </summary>
        Task<IEnumerable<OldTaskDto>> GetAllTasksOldDto();
        /// <summary> Получение списка всех задач </summary>
        Task<IEnumerable<TaskDto>> GetAllTasks();
        /// <summary> Получение списка задач (по названию) </summary>
        Task<IEnumerable<TaskDto>> GetTasksByTaskNameSubStr(string taskNameSubStr);
        /// <summary> Получение списка актуальных задач (по сроку) </summary>
        Task<IEnumerable<TaskDto>> GetAllHotTasksByDeadLine(DateTime deadLine);
        /// <summary> Получение списка всех актуальных задач </summary>
        Task<IEnumerable<TaskDto>> GetAllHotTasks();
        /// <summary> Получение списка задач по проекту </summary>
        Task<IEnumerable<TaskDto>> GetTasksForProject(string projectId, string taskNameSubStr = null);
        /// <summary> Получение задачи по taskId </summary>
        Task<TaskDto> GetTaskById(int taskId);

        /// <summary> Обновление задачи </summary>
        Task<string> UpdateTask(TaskDto taskDto);
        
        /// <summary> Удаление задачи </summary>
        Task<string> DeleteTask(DeleteTaskRequestDto deleteTaskRequest);
    }
}
