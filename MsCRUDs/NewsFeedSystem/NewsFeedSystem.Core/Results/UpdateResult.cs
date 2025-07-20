using System.Net;

namespace NewsFeedSystem.Core.Results
{
    public class UpdateResult
    {
        /// <summary> Сообщение о результате обновления </summary>
        public string Message { get; set; } = default!;

        public HttpStatusCode StatusCode { get; set; }

        public UpdateResult()
        { }

        public UpdateResult(string message, HttpStatusCode statusCode)
        {
            Message = message;
            StatusCode = statusCode;
        }
    }
}
