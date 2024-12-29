using System.Collections.Generic;

namespace ProjectTasksTrackService.Contracts
{
    public class Project
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string ImageURL { get; set; }
        public List<byte> ScheduledDayNums { get; set; }
    }
}
