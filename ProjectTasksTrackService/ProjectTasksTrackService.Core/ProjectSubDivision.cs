using System;

namespace ProjectTasksTrackService.Core
{
    /// <summary>
    /// Класс модели подпроекта (направления/подраздела/модуля/части) 
    /// </summary>
    public class ProjectSubDivision
    {
        /// <summary> Id подпроекта (направления/подраздела/модуля/части) (ProjectSubDivisionId) </summary>
        public int Id { get { return _id; } }
        /// <summary> Id проекта (родительского) </summary>
        public int ProjectId { get { return _projectId; } }
        public string Code { get { return _code; } }
        /// <summary> Числовой идентификатор (номер) проекта, как в старой системе </summary>
        public string Name { get { return _name; } }
        public string Url1 { get { return _url1; } }
        public string Url2 { get { return _url2; } }
        public string ImageUrl { get { return _imageUrl; } }
        public DateTime? CreatedDt { get { return _createdDt; } }
        public DateTime? LastUpdateDt { get { return _lastUpdateDt; } }
        public DateTime? DeadLineDt { get { return _deadLineDt; } }

        /// <summary> Id подпроекта (направления/подраздела/модуля/части) (ProjectSubDivisionId) </summary>
        private readonly int _id;
        /// <summary> Числовой идентификатор (номер) (родительского) проекта, как в старой системе </summary>
        private readonly int _projectId;
        private string _code;
        private string _name;
        private string _url1;
        private string _url2;
        private string _imageUrl;
        private DateTime? _createdDt;
        private DateTime? _lastUpdateDt;
        private DateTime? _deadLineDt;
        public ProjectSubDivision(
            int id,
            int projectId,
            string code,
            string name,
            string url1 = null,
            string url2 = null,
            string imageUrl = null,
            DateTime? createdDt = null,
            DateTime? lastUpdateDt = null,
            DateTime? deadLineDt = null
            )
        {
            _id = id;
            _projectId = projectId;
            _code = code;
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
        public void UpdateCreatedDt(DateTime? createdDt) => _createdDt = createdDt;
        public void UpdateLastUpdateDt(DateTime? lastUpdateDt) => _lastUpdateDt = lastUpdateDt;
        public void UpdateDeadLineDt(DateTime? deadLineDt) => _deadLineDt = deadLineDt;
    }
}
