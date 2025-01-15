using System.Text.Json.Serialization;

namespace ProjectTasksTrackService.API.Contracts.Dto
{
    /// <summary> Legacy-класс-модель направления (подраздела/модуля/части) проекта в API для импорта/экспорта в старую систему </summary>
    public class OldProjectSubDivisionDto
    {
        /// <summary> Id направления (подраздела/модуля/части) проекта </summary>
        [JsonPropertyName("i")] public required int Id { get; set; }

        /// <summary> Название направления (подраздела/модуля/части) проекта </summary>
        [JsonPropertyName("n")] public required string Name { get; set; }

        /// <summary> Дата и время создания </summary>
        [JsonPropertyName("cr")] public string CreatedDt { get; set; }
        
        /// <summary> Дата и время изменения </summary>
        [JsonPropertyName("la")] public string LastUpdateDt { get; set; }

        /// <summary> Флаг готовности подраздела/модуля/части проекта </summary>
        [JsonPropertyName("fin")]
        public bool IsFinished { get; set; }

        /// <summary> Legacy-идентификатор (номер) проекта в старой системе </summary>
        [JsonPropertyName("m")]
        public required int LegacyRootProjectNumber { get; set; }

        /// <summary> Id проекта </summary>
        [JsonPropertyName("uid")]
        public string ProjectId { get; set; }
    }
}
