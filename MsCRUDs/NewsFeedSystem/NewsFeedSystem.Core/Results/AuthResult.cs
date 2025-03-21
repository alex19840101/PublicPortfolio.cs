using System.Net;

namespace NewsFeedSystem.Core.Results
{
    public class AuthResult
    {
        /// <summary> Идентификатор пользователя </summary>
        public int? Id { get; set; }

        /// <summary> Сообщение о результате выполнения запроса </summary>
        public string Message { get; set; }

        public HttpStatusCode StatusCode { get; set; }
        public string Token { get; set; }

        public AuthResult()
        { }

        public AuthResult(string message,
            HttpStatusCode statusCode,
            int? id = null,
            string token = null)
        {
            Message = message;
            StatusCode = statusCode;
            Id = id;
            Token = token;
        }
    }
}
