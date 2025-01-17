using System.Collections.Generic;

namespace ProjectTasksTrackService.DataAccess.Entities
{
    /// <summary> Entity-класс проекта </summary>
    public class Project
    {
        /// <summary> Строковый идентификатор проекта </summary>
        public string Id { get { return _id; } }
        /// <summary> Числовой идентификатор (номер) проекта, как в старой системе </summary>
        public int IntId { get { return _intId; } }
        public string Name { get { return _name; } }
        public string Url { get { return _url; } }
        public string ImageUrl { get { return _imageUrl; } }
        /// <summary> Номера дней обязательной активности по проекту (совещаний) </summary>
        public HashSet<byte> ScheduledDayNums { get { return _scheduledDayNums; } }
        public string CreatedDt { get { return _createdDt; } }
        public string LastUpdateDt { get { return _lastUpdateDt; } }

        /// <summary> Навигационное свойство -> список задач проекта </summary>
        public ICollection<ProjectTask> Tasks { get; set; } = [];
        /// <summary> Навигационное свойство -> список подпроектов </summary>
        public ICollection<ProjectSubDivision> ProjectSubDivisions { get; set; } = [];

        private readonly string _id;
        private readonly int _intId;
        private string _name;
        private string _url;
        private string _imageUrl;
        private HashSet<byte> _scheduledDayNums;
        private string _createdDt;
        private string _lastUpdateDt;
        public Project(
            string projectId,
            string name,
            int intProjectId,
            string url = null,
            string imageUrl = null,
            HashSet<byte> scheduledDayNums = null,
            string createdDt = null,
            string lastUpdateDt = null
            )
        {
            _id = projectId;
            _intId = intProjectId;
            _name = name;
            _url = url;
            _imageUrl = imageUrl;
            _scheduledDayNums = scheduledDayNums;
            _createdDt = createdDt;
            _lastUpdateDt = lastUpdateDt;
        }

        public void UpdateName(string newName) => _name = newName;
        public void UpdateUrl(string newUrl) => _url = newUrl;
        public void UpdateImageUrl(string newImageUrl) => _imageUrl = newImageUrl;
        public void UpdateScheduledDayNums(HashSet<byte> scheduledDayNums) => _scheduledDayNums = scheduledDayNums;
        public void UpdateCreatedDt(string createdDt) => _createdDt = createdDt;
        public void UpdateLastUpdateDt(string lastUpdateDt) => _lastUpdateDt = lastUpdateDt;
    }
}
