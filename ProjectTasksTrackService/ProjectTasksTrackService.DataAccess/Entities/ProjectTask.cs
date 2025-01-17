namespace ProjectTasksTrackService.DataAccess.Entities
{
    /// <summary>
    /// Entity-класс задачи по проекту
    /// </summary>
    public class ProjectTask
    {
        /// <summary> Id задачи (события/напоминания) в старой системе </summary>
        public int Id { get; set; }
        public string ProjectId { get { return _projectId; } }
        public int? ProjectSubDivisionId { get { return _projectSubDivisionId; } }
        /// <summary> Числовой идентификатор (номер) проекта, как в старой системе  - внешний ключ (Project.IntId) </summary>
        public int ProjectIntId { get { return _projectIntId; } }
        public string Name { get { return _name; } }
        public string Url1 { get { return _url1; } }
        public string Url2 { get { return _url2; } }
        public string ImageUrl { get { return _imageUrl; } }
        public string CreatedDt { get { return _createdDt; } }
        public string LastUpdateDt { get { return _lastUpdateDt; } }
        public string DeadLineDt { get { return _deadLineDt; } }

        private readonly string _projectId;
        private readonly int? _projectSubDivisionId;
        /// <summary> Числовой идентификатор (номер) проекта, как в старой системе  - внешний ключ (Project.IntId) </summary>
        private readonly int _projectIntId;
        private string _name;
        private string _url1;
        private string _url2;
        private string _imageUrl;
        private string _createdDt;
        private string _lastUpdateDt;
        private string _deadLineDt;
        public ProjectTask(
            string projectId,
            string name,
            int intProjectId,
            int? projectSubDivisionId = null,
            string url1 = null,
            string url2 = null,
            string imageUrl = null,
            string createdDt = null,
            string lastUpdateDt = null,
            string deadLineDt = null
            )
        {
            _projectId = projectId;
            _projectIntId = intProjectId;
            _projectSubDivisionId = projectSubDivisionId;
            _name = name;
            _url1 = url1;
            _url2 = url2;
            _imageUrl = imageUrl;
            _createdDt = createdDt;
            _lastUpdateDt = lastUpdateDt;
            _deadLineDt = deadLineDt;
        }

        public void UpdateName(string newName) => _name = newName;
        public void UpdateUrl1(string newUrl1) => _url1 = newUrl1;
        public void UpdateUrl2(string newUrl2) => _url2 = newUrl2;
        public void UpdateImageUrl(string newImageUrl) => _imageUrl = newImageUrl;
        public void UpdateCreatedDt(string createdDt) => _createdDt = createdDt;
        public void UpdateLastUpdateDt(string lastUpdateDt) => _lastUpdateDt = lastUpdateDt;
        public void UpdateDeadLineDt(string deadLineDt) => _deadLineDt = deadLineDt;
    }
}
