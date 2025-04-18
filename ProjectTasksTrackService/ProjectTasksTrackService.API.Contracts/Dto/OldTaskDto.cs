﻿using System;
using System.Text.Json.Serialization;
using ProjectTasksTrackService.API.Contracts.Dto.Enums;

namespace ProjectTasksTrackService.API.Contracts.Dto
{
    /// <summary> Legacy-класс-модель задачи в API для импорта/экспорта в старую систему </summary>
    public class OldTaskDto
    {
        /// <summary> Id задачи (события/напоминания) в старой системе </summary>
        [JsonPropertyName("e")] public int Id { get; set; }

        /// <summary> Числовой идентификатор - номер проекта, как в старой системе </summary>
        [JsonPropertyName("id")] public required int ProjectId { get; set; }

        /// <summary> Код задачи (генерируется автоматически: {Project.Code}[.{Subdivision.Code}].{Task.Id})</summary>
        [JsonPropertyName("c")] public string Code { get; set; }

        /// <summary> Название задачи/события/напоминания </summary>
        [JsonPropertyName("n")] public required string Name { get; set; }

        /// <summary> Дата и время создания задачи </summary>
        [JsonPropertyName("cr")] public DateTime? CreatedDt { get; set; }

        /// <summary> Срок (дата и время завершения) задачи по плану </summary>
        [JsonPropertyName("dt")] public DateTime? DeadLineDt { get; set; }
        
        /// <summary> Дата и время завершения задачи </summary>
        [JsonPropertyName("done")] public DateTime? DoneDateTime { get; set; }

        /// <summary> Изображение </summary>
        [JsonPropertyName("i")] public string ImageUrl { get; set; }

        /// <summary> Повторяемость задачи по TaskRepeatsType </summary>
        [JsonPropertyName("r")] public TaskRepeatsType RepeatsType { get; set; }
        
        /// <summary> Через (...) дней повторять </summary>
        [JsonPropertyName("rd")] public ushort? RepeatInDays { get; set; }

        /// <summary> Ссылка #1 ((локальная)) </summary>
        [JsonPropertyName("u1")] public string Url1 { get; set; }
        
        /// <summary> Ссылка #2 ((Интернет)) </summary>
        [JsonPropertyName("u2")] public string Url2 { get; set; }
        
        /// <summary> Дата и время изменения </summary>
        [JsonPropertyName("tla")] public DateTime? LastUpdateDt { get; set; }
        
        /// <summary> Id направления (подраздела/модуля/части) проекта </summary>
        [JsonPropertyName("su")] public int? ProjectSubDivisionId { get; set; }
    }
}
