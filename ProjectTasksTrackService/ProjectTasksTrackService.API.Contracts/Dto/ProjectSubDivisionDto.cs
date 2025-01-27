using System;
using System.Text.Json.Serialization;

namespace ProjectTasksTrackService.API.Contracts.Dto
{
    /// <summary> Направление (подраздел/модуль/часть) проекта </summary>
    public class ProjectSubDivisionDto
    {
        /// <summary> Id направления (подраздела/модуля/части) проекта </summary>
        public int Id { get; set; }

        /// <summary> Числовой идентификатор - номер проекта, как в старой системе </summary>
        public required int ProjectId { get; set; }

        /// <summary> Код направления (подраздела/модуля/части) </summary>
        [JsonPropertyName("c")] public required string Code { get; set; }

        /// <summary> Название направления (подраздела/модуля/части) проекта </summary>
        public required string Name { get; set; }

        /// <summary> Дата и время создания </summary>
        public DateTime? CreatedDt { get; set; }
        
        /// <summary> Дата и время изменения </summary>
        public DateTime? LastUpdateDt { get; set; }

        /// <summary> Дата и время завершения подраздела/модуля/части проекта </summary>
        public DateTime? DoneDateTime { get; set; }

        /// <summary> Флаг готовности подраздела/модуля/части проекта </summary>
        public bool IsFinished { get { return DoneDateTime != null; } }

        /// <summary> Ссылка #1 ((локальная)) </summary>
        public string Url1 { get; set; }

        /// <summary> Ссылка #2 ((Интернет)) </summary>
        public string Url2 { get; set; }

        /// <summary> Изображение </summary>
        public string ImageUrl { get; set; }
        
        /// <summary> Срок (дата и время завершения) подпроекта (при необходимости) </summary>
        public DateTime? DeadLineDt { get; set; }
    }
}
