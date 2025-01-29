using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ProjectTasksTrackService.Core.Results
{
    public class CreateResult
    {
        /// <summary> Идентификатор добавленной сущности </summary>
        public int? Id { get; set; }

        /// <summary> Сообщение о результате выполнения запроса Create </summary>
        public string Message { get; set; }

        /// <summary> Сообщение о результате выполнения запроса Create </summary>
        public string Code { get; set; }

        public HttpStatusCode StatusCode { get; set; }
        public string SecretString { get; set; }

        public CreateResult()
        { }

        public CreateResult(string message,
            HttpStatusCode statusCode,
            int? id = null,
            string secretString = null,
            string code = null)
        {
            Message = message;
            StatusCode = statusCode;
            Id = id;
            SecretString = secretString;
            Code = code;
        }
    }
}
