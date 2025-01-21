using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTasksTrackService.DataAccess.Entities
{
    /// <summary> Entity-класс проекта </summary>
    public class Project
    {
        /// <summary> Числовой идентификатор - номер проекта, как в старой системе </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get { return _id; } }
        /// <summary> Строковый идентификатор проекта </summary>
        public string Code { get { return _code; } }
        public string Name { get { return _name; } }
        public string Url { get { return _url; } }
        public string ImageUrl { get { return _imageUrl; } }
        public DateTime? CreatedDt { get { return _createdDt; } }
        public DateTime? LastUpdateDt { get { return _lastUpdateDt; } }

        /// <summary> Навигационное свойство -> список задач проекта </summary>
        public ICollection<ProjectTask> Tasks { get; set; } = [];
        /// <summary> Навигационное свойство -> список подпроектов </summary>
        public ICollection<ProjectSubDivision> ProjectSubDivisions { get; set; } = [];

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
    }
}
