using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectTasksTrackService.API.Contracts.Dto;
using ProjectTasksTrackService.API.Contracts.Dto.Enums;

namespace ProjectTasksTrackService.API.Contracts.Interfaces
{
    /// <summary> Интерфейс контроллера управления задачами </summary>
    public interface ITasksAPI
    {
        /// <summary> Импорт задач (из старой системы) </summary>
        Task<string> Import(IEnumerable<OldProjectDto> projects);
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
        Task<IEnumerable<TaskDto>> GetTasksByProjectId(string projectId, string taskNameSubStr = null);
        /// <summary> Получение задачи по taskId </summary>
        Task<TaskDto> GetTaskById(int taskId);

        /// <summary> Обновление задачи </summary>
        Task<string> UpdateTask(TaskDto taskDto);
        /// <summary> Обновление названия задачи </summary>
        Task<string> UpdateName(int taskId, string newName);
        /// <summary> Обновление срока задачи </summary>
        Task<string> UpdateDeadLine(int taskId, DateTime deadLine);
        /// <summary> Обновление даты и времени завершения задачи </summary>
        Task<string> UpdateDoneDateTime(int taskId, DateTime deadLine);
        /// <summary> Обновление ссылки (url1) задачи </summary>
        Task<string> UpdateUrl1(int taskId, string url);
        /// <summary> Обновление ссылки (url2) задачи </summary>
        Task<string> UpdateUrl2(int taskId, string url);
        /// <summary> Обновление изображения (imageUrl) задачи </summary>
        Task<string> UpdateImageUrl(int taskId, string imageUrl);
        /// <summary> Обновление RepeatsType задачи </summary>
        Task<string> UpdateRepeatOptions(int taskId, TaskRepeatsType repeatsType, ushort? repeatInDays = null);
        
        /// <summary> Удаление задачи </summary>
        Task<string> DeleteTask(int taskId, string taskSecretString);
    }
}
