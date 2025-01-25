using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTasksTrackService.Core.Results
{
    public class ImportResult
    {
        /// <summary> Количество импортированных (добавленных) проектов </summary>
        public int ImportedCount { get; set; }

        /// <summary> Сообщение о результате импорта </summary>
        public string Message { get; set; }
    }
}
