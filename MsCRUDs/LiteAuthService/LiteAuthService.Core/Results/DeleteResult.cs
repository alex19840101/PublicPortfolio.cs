using System.Net;

namespace LiteAuthService.Core.Results
{
    public class DeleteResult
    {
        /// <summary> Сообщение о результате удаления </summary>
        public string Message { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public DeleteResult()
        { }

        public DeleteResult(string message, HttpStatusCode statusCode)
        {
            Message = message;
            StatusCode = statusCode;
        }
    }
}
