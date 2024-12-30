using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ProjectTasksTrackService.API.Contracts.Dto
{
    /// <summary> Legacy-класс для импорта/экспорта в старую систему </summary>
    public class OldProjectDto
    {
        [JsonPropertyName("uid")] public string ProjectId { get; set; }
        /// <summary> Legacy-идентификатор (номер) проекта в старой системе </summary>
        [JsonPropertyName("id")] public int LegacyProjectNumber { get; set; }
        [JsonPropertyName("n")] public string Name { get; set; }
        [JsonPropertyName("u")] public string URL { get; set; }
        [JsonPropertyName("i")] public string ImageURL { get; set; }
        /// <summary> Номера дней обязательной активности по проекту (совещаний) </summary>
        [JsonPropertyName("s")] public HashSet<byte> ScheduledDayNums { get; set; }
    }
}
