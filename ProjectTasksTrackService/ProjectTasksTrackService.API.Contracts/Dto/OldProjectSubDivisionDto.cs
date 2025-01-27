using System;
using System.Text.Json.Serialization;

namespace ProjectTasksTrackService.API.Contracts.Dto
{
    /// <summary> Legacy-класс-модель направления (подраздела/модуля/части) проекта в API для импорта/экспорта в старую систему </summary>
    public class OldProjectSubDivisionDto
    {
        /// <summary> Id направления (подраздела/модуля/части) проекта </summary>
        [JsonPropertyName("i")] public int Id { get; set; }

        /// <summary> Числовой идентификатор (номер) проекта, как в старой системе </summary>
        [JsonPropertyName("m")]
        public required int ProjectId { get; set; }
        /// <summary> Код направления (подраздела/модуля/части) </summary>
        [JsonPropertyName("c")] public required string Code { get; set; }
        /// <summary> Название направления (подраздела/модуля/части) проекта </summary>
        [JsonPropertyName("n")] public required string Name { get; set; }

        /// <summary> Дата и время создания </summary>
        [JsonPropertyName("cr")] public DateTime? CreatedDt { get; set; }
        
        /// <summary> Дата и время изменения </summary>
        [JsonPropertyName("la")] public DateTime? LastUpdateDt { get; set; }

        /// <summary> Дата и время завершения задачи </summary>
        [JsonPropertyName("done")] public DateTime? DoneDateTime { get; set; }

        /// <summary> Флаг готовности подраздела/модуля/части проекта </summary>
        [JsonPropertyName("fin")]
        public bool IsFinished { get { return DoneDateTime != null; } }

        /// <summary> Ссылка #1 ((локальная)) </summary>
        [JsonPropertyName("u1")] public string Url1 { get; set; }

        /// <summary> Ссылка #2 ((Интернет)) </summary>
        [JsonPropertyName("u2")] public string Url2 { get; set; }

        /// <summary> Изображение </summary>
        [JsonPropertyName("im")] public string ImageUrl { get; set; }

        /// <summary> Срок (дата и время завершения) подпроекта (при необходимости) </summary>
        [JsonPropertyName("dt")] public DateTime? DeadLineDt { get; set; }
    }
}
