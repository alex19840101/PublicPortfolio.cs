using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ProjectTasksTrackService.Core.Results
{
    public class UpdateResult
    {
        /// <summary> Сообщение о результате обновления </summary>
        public string Message { get; set; }

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
