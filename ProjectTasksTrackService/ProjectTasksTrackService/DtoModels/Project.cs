using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ProjectTasksTrackService.API.DtoModels
{
    public class Project
    {
        [JsonPropertyName("id")] public byte Id { get; set; }
        [JsonPropertyName("n")] public string Name { get; set; }
        [JsonPropertyName("u")] public string URL { get; set; }
        [JsonPropertyName("i")] public string ImageURL { get; set; }
        [JsonPropertyName("s")] public List<byte> ScheduledDayNums { get; set; }
    }
}
