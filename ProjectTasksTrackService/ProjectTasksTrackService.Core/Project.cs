﻿using System.Collections.Generic;

namespace ProjectTasksTrackService.Core
{
    public class Project
    {
        public string ProjectId { get { return _projectId; } }
        /// <summary> Legacy-идентификатор (номер) проекта в старой системе </summary>
        public int? LegacyProjectNumber { get { return _legacyProjectNumber; } }
        public string Name { get { return _name; } }
        public string Url { get { return _url; } }
        public string ImageURL { get { return _imageUrl; } }
        /// <summary> Номера дней обязательной активности по проекту (совещаний) </summary>
        public HashSet<byte> ScheduledDayNums { get { return _scheduledDayNums; } }

        private readonly string _projectId;
        private readonly int? _legacyProjectNumber;
        private string _name;
        private string _url;
        private string _imageUrl;
        private HashSet<byte> _scheduledDayNums;

        public Project(
            string projectId,
            string name,
            int? legacyProjectNumber = null,
            string url = null,
            string imageUrl = null,
            HashSet<byte> scheduledDayNums = null
            )
        {
            _projectId = projectId;
            _legacyProjectNumber = legacyProjectNumber;
            _name = name;
            _url = url;
            _imageUrl = imageUrl;
            _scheduledDayNums = scheduledDayNums;
        }

        public void UpdateName(string newName) => _name = newName;
        public void UpdateUrl(string newUrl) => _url = newUrl;
        public void UpdateImageUrl(string newImageUrl) => _imageUrl = newImageUrl;
        public void UpdateScheduledDayNums(HashSet<byte> scheduledDayNums) => _scheduledDayNums = scheduledDayNums;
    }
}
