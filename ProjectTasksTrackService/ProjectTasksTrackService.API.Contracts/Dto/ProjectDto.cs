using System.Collections.Generic;

namespace ProjectTasksTrackService.API.Contracts.Dto
{
    public class ProjectDto
    {
        public string ProjectId { get; set; }
        /// <summary> Legacy-идентификатор (номер) проекта в старой системе </summary>
        public int LegacyProjectNumber { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        /// <summary> Номера дней обязательной активности по проекту (совещаний) </summary>
        public HashSet<byte> ScheduledDayNums { get; set; }
    }
}
