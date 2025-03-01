using System.Net;

namespace ProjectTasksTrackService.Core.Results
{
    public class ImportResult
    {
        /// <summary> Количество импортированных (добавленных) проектов </summary>
        public int ImportedCount { get; set; }

        /// <summary> Сообщение о результате импорта </summary>
        public string Message { get; set; }

        public HttpStatusCode StatusCode { get; set; }
        public ImportResult()
        { }

        public ImportResult(string message, HttpStatusCode statusCode, int importedCount = 0)
        {
            Message = message;
            StatusCode = statusCode;
            ImportedCount = importedCount;
        }
    }
}
