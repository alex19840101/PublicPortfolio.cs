using System.Net;

namespace ShopServices.Abstractions
{
    /// <summary>
    /// Класс с результатом обработки запроса на создание/изменение/удаление
    /// </summary>
    public class Result
    {
        /// <summary> Id сущности </summary>
        public uint? Id { get; set; }

        /// <summary> Сообщение о результате создания/изменения/удаления </summary>
        public string Message { get; set; }

        /// <summary> HttpStatusCode-код </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary> Результат обработки запроса на создание/изменение/удаление </summary>
        public Result()
        { }

        /// <summary> Результат обработки запроса на создание/изменение/удаление </summary>
        public Result(string message,
            HttpStatusCode statusCode,
            uint? id = null)
        {
            Message = message;
            StatusCode = statusCode;
            Id = id;
        }
    }
}
