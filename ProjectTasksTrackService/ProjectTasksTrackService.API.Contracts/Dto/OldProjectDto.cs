using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ProjectTasksTrackService.API.Contracts.Dto
{
    /// <summary> Legacy-класс-модель проекта в API для импорта/экспорта в старую систему </summary>
    public class OldProjectDto
    {
        /// <summary> Id проекта </summary>
        [JsonPropertyName("uid")] public string ProjectId { get; set; }
        
        /// <summary> Legacy-идентификатор (номер) проекта в старой системе </summary>
        [JsonPropertyName("id")] public int LegacyProjectNumber { get; set; }
        
        /// <summary> Название проекта </summary>
        [JsonPropertyName("n")] public string Name { get; set; }
        
        /// <summary> URL проекта </summary>
        [JsonPropertyName("u")] public string Url { get; set; }
        
        /// <summary> Логотип (эмблема) (imageUrl) проекта </summary>
        [JsonPropertyName("i")] public string ImageUrl { get; set; }
        
        /// <summary> Номера дней обязательной активности по проекту (совещаний) </summary>
        [JsonPropertyName("s")] public HashSet<byte> ScheduledDayNums { get; set; }
    }
}
