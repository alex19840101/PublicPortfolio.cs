using System.Collections.Generic;

namespace ProjectTasksTrackService.DataAccess.Entities
{
    /// <summary>
    /// Entity-класс подпроекта (направления/подраздела/модуля/части) 
    /// </summary>
    public class ProjectSubDivision
    {
        /// <summary> Id подпроекта (направления/подраздела/модуля/части) (ProjectSubDivisionId) </summary>
        public int Id { get { return _id; } }
        /// <summary> Id проекта (родительского) - внешний ключ (Project.Id) </summary>
        public string ProjectId { get { return _projectId; } }
        /// <summary> Числовой идентификатор (номер) проекта, как в старой системе - внешний ключ (Project.IntId) </summary>
        public int ProjectIntId { get { return _projectIntId; } }
        public string Name { get { return _name; } }
        public string Url1 { get { return _url1; } }
        public string Url2 { get { return _url2; } }
        public string ImageUrl { get { return _imageUrl; } }
        public string CreatedDt { get { return _createdDt; } }
        public string LastUpdateDt { get { return _lastUpdateDt; } }
        public string DeadLineDt { get { return _deadLineDt; } }

        /// <summary> Навигационное свойство -> список задач подпроекта </summary>
        public List<ProjectTask> Tasks { get; set; } = [];

        /// <summary> Id подпроекта (направления/подраздела/модуля/части) (ProjectSubDivisionId) </summary>
        private readonly int _id;
        /// <summary> Id проекта (родительского) </summary>
        private readonly string _projectId;
        /// <summary> Числовой идентификатор (номер) (родительского) проекта, как в старой системе - внешний ключ (Project.IntId) </summary>
        private readonly int _projectIntId;
        private string _name;
        private string _url1;
        private string _url2;
        private string _imageUrl;
        private string _createdDt;
        private string _lastUpdateDt;
        private string _deadLineDt;
        public ProjectSubDivision(
            int id,
            string projectId,
            string name,
            int intProjectId,
            string url1 = null,
            string url2 = null,
            string imageUrl = null,
            string createdDt = null,
            string lastUpdateDt = null,
            string deadLineDt = null
            )
        {
            _id = id;
            _projectId = projectId;
            _projectIntId = intProjectId;
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
