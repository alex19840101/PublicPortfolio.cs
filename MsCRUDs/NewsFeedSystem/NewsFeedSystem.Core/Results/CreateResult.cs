﻿using System.Net;

namespace NewsFeedSystem.Core.Results
{
    public class CreateResult
    {
        /// <summary> Идентификатор добавленной сущности </summary>
        public uint? Id { get; set; }

        /// <summary> Сообщение о результате выполнения запроса Create </summary>
        public string Message { get; set; } = default!;


        public HttpStatusCode StatusCode { get; set; }

        public CreateResult()
        { }

        public CreateResult(string message,
            HttpStatusCode statusCode,
            uint? id = null)
        {
            Message = message;
            StatusCode = statusCode;
            Id = id;
        }
    }
}
