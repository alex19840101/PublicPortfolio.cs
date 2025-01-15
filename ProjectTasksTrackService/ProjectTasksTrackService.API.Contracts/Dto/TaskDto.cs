﻿using ProjectTasksTrackService.API.Contracts.Dto.Enums;

namespace ProjectTasksTrackService.API.Contracts.Dto
{
    /// <summary> Класс-модель задачи по проекту </summary>
    public class TaskDto
    {
        /// <summary> Id задачи (события/напоминания) в старой системе </summary>
        public required int Id { get; set; }

        /// <summary> Id проекта </summary>
        public string ProjectId { get; set; }

        /// <summary> Id проекта в старой системе </summary>
        public required int LegacyRootProjectNumber { get; set; }

        /// <summary> Название задачи/события/напоминания </summary>
        public required string Name { get; set; }

        /// <summary> Дата и время создания задачи </summary>
        public string CreatedDt { get; set; }

        /// <summary> Срок (дата и время завершения) задачи по плану </summary>
        public string Deadline { get; set; }
        
        /// <summary> Дата и время завершения задачи </summary>
        public string DoneDateTime { get; set; }

        /// <summary> Изображение </summary>
        public string ImageUrl { get; set; }

        /// <summary> Повторяемость задачи </summary>
        public TaskRepeatsType RepeatsType { get; set; }
        
        /// <summary> Через (...) дней повторять </summary>
        public ushort? RepeatInDays { get; set; }

        /// <summary> Ссылка #1 ((локальная)) </summary>
        public string Url1 { get; set; }
        
        /// <summary> Ссылка #2 ((Интернет)) </summary>
        public string Url2 { get; set; }
    }
}
