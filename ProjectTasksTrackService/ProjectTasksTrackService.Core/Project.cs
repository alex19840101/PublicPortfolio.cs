using System;
using System.Collections.Generic;

namespace ProjectTasksTrackService.Core
{
    public class Project
    {
        /// <summary> Числовой идентификатор - номер проекта, как в старой системе </summary>
        public int Id { get { return _id; } }
        public string Code { get { return _code; } }
        public string Name { get { return _name; } }
        public string Url { get { return _url; } }
        public string ImageUrl { get { return _imageUrl; } }
        public DateTime? CreatedDt { get { return _createdDt; } }
        public DateTime? LastUpdateDt { get { return _lastUpdateDt; } }

        private readonly int _id;
        private readonly string _code;
        private string _name;
        private string _url;
        private string _imageUrl;
        private DateTime? _createdDt;
        private DateTime? _lastUpdateDt;
        public Project(
            int id,
            string code,
            string name,
            string url = null,
            string imageUrl = null,
            DateTime? createdDt = null,
            DateTime? lastUpdateDt = null
            )
        {
            _id = id;
            _code = code;
            _name = name;
            _url = url;
            _imageUrl = imageUrl;
            _createdDt = createdDt;
            _lastUpdateDt = lastUpdateDt;
        }

        public void UpdateName(string newName) => _name = newName;
        public void UpdateUrl(string newUrl) => _url = newUrl;
        public void UpdateImageUrl(string newImageUrl) => _imageUrl = newImageUrl;
        public void UpdateCreatedDt(DateTime? createdDt) => _createdDt = createdDt;
        public void UpdateLastUpdateDt(DateTime? lastUpdateDt) => _lastUpdateDt = lastUpdateDt;

        public override bool Equals(object obj)
        {
            var comparedProject = (Project)obj;
            if (comparedProject.Id != _id ||
                !string.Equals(comparedProject.Code, _code) ||
                !string.Equals(comparedProject.Name, _name) ||
                !string.Equals(comparedProject.Url, _url) ||
                !string.Equals(comparedProject.ImageUrl, _imageUrl) ||
                comparedProject.CreatedDt != _createdDt ||
                comparedProject.LastUpdateDt != _lastUpdateDt)
                return false;

            return true;
        }
    }
}
