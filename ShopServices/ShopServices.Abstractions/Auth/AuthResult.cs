using System.Net;

namespace ShopServices.Abstractions.Auth
{
    /// <summary> Результат регистрации/залогинивания </summary>
    public class AuthResult
    {
        /// <summary> Идентификатор пользователя </summary>
        public uint? Id { get; set; }

        /// <summary> Сообщение о результате выполнения запроса </summary>
        public string Message { get; set; } = default!;

        /// <summary> HttpStatusCode - HTTP-статус код для ответа сервиса </summary>
        public HttpStatusCode StatusCode { get; set; }
        /// <summary> Токен </summary>
        public string Token { get; set; }

        /// <summary> Конструктор класса результата регистрации/залогинивания </summary>
        public AuthResult()
        { }

        /// <summary> Конструктор класса результата регистрации/залогинивания </summary>
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
