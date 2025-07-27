using System.Net;

namespace ShopServices.Abstractions
{
    /// <summary>
    /// Класс с результатом обработки запроса на создание/изменение/удаление
    /// </summary>
    public class Result
    {
        /// <summary> Id сущности </summary>
        public ulong? Id { get; set; }

        /// <summary> Id покупателя (для уведомлений покупателя) </summary>
        public uint? BuyerId { get; set; }

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
            uint? id = null,
            uint? buyerId = null)
        {
            Message = message;
            StatusCode = statusCode;
            Id = id;
            BuyerId = buyerId;
        }
    }
}
