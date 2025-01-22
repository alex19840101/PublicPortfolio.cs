using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ProjectTasksTrackService.API.Contracts.Dto
{
    /// <summary> Legacy-класс-модель проекта в API для импорта/экспорта в старую систему </summary>
    public class OldProjectDto
    {
        /// <summary> Числовой идентификатор - номер проекта, как в старой системе </summary>
        [JsonPropertyName("id")] public int Id { get; set; }

        /// <summary> Код проекта </summary>
        [JsonPropertyName("c")] public required string Code { get; set; }
        
        /// <summary> Название проекта </summary>
        [JsonPropertyName("n")] public required string Name { get; set; }
        
        /// <summary> URL проекта </summary>
        [JsonPropertyName("u")] public string Url { get; set; }
        
        /// <summary> Логотип (эмблема) (imageUrl) проекта </summary>
        [JsonPropertyName("i")] public string ImageUrl { get; set; }
        
        /// <summary> Дата и время создания </summary>
        [JsonPropertyName("pcr")] public DateTime? CreatedDt { get; set; }

        /// <summary> Дата и время изменения </summary>
        [JsonPropertyName("pla")] public DateTime? LastUpdateDt { get; set; }
    }
}
