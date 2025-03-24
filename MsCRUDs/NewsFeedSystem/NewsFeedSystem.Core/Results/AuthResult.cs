using System.Net;

namespace NewsFeedSystem.Core.Results
{
    public class AuthResult
    {
        /// <summary> Идентификатор пользователя </summary>
        public uint? Id { get; set; }

        /// <summary> Сообщение о результате выполнения запроса </summary>
        public string Message { get; set; }

        public HttpStatusCode StatusCode { get; set; }
        public string Token { get; set; }

        public AuthResult()
        { }

        public AuthResult(string message,
            HttpStatusCode statusCode,
            uint? id = null,
            string token = null)
        {
            Message = message;
            StatusCode = statusCode;
            Id = id;
            Token = token;
        }
    }
}
