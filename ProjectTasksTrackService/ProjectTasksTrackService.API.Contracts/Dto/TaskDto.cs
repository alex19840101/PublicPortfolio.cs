using System;
using ProjectTasksTrackService.API.Contracts.Dto.Enums;

namespace ProjectTasksTrackService.API.Contracts.Dto
{
    /// <summary> Класс-модель задачи по проекту </summary>
    public class TaskDto
    {
        /// <summary> Id задачи (события/напоминания) в старой системе </summary>
        public int Id { get; set; }

        /// <summary> Числовой идентификатор - номер проекта, как в старой системе </summary>
        public required int ProjectId { get; set; }

        /// <summary> Название задачи/события/напоминания </summary>
        public required string Name { get; set; }

        /// <summary> Дата и время создания задачи </summary>
        public DateTime? CreatedDt { get; set; }

        /// <summary> Срок (дата и время завершения) задачи по плану </summary>
        public DateTime? DeadLineDt { get; set; }
        
        /// <summary> Дата и время завершения задачи </summary>
        public DateTime? DoneDateTime { get; set; }

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

        /// <summary> Дата и время изменения </summary>
        public DateTime? LastUpdateDt { get; set; }
        
        /// <summary> Id направления (подраздела/модуля/части) проекта </summary>
        public int? ProjectSubDivisionId { get; set; }
    }
}
