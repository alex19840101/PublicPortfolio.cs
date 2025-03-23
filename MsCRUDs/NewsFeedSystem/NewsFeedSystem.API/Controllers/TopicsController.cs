using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsFeedSystem.API.Contracts.Responses;
using NewsFeedSystem.API.Contracts.Interfaces;
using NewsFeedSystem.API.Contracts.Requests;
using NewsFeedSystem.Core.Results;
using NewsFeedSystem.API.Contracts;

namespace NewsFeedSystem.API.Controllers
{
    [ApiController]
    [Asp.Versioning.ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class TopicsController : ControllerBase, ITopicsAPI
    {
        private readonly ILogger<TopicsController> _logger;

        public TopicsController(ILogger<TopicsController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateResponseDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> Create(TopicDto request)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public Task<IActionResult> Get(int topicId)
        {
            throw new NotImplementedException();
        }


        [ProducesResponseType(typeof(TopicDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [HttpGet]
        public Task<IEnumerable<TopicDto>> GetTopics(int? maxTopicId, int? minTopicId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ProducesResponseType(typeof(UpdateResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(UpdateResult), (int)HttpStatusCode.NotFound)]
        public Task<IActionResult> Update(TopicDto request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Удаление темы
        /// </summary>
        /// <param name="topicId"> Id темы </param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(DeleteResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(DeleteResult), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(DeleteResult), (int)HttpStatusCode.NotFound)]
        public Task<IActionResult> Delete(int topicId)
        {
            throw new NotImplementedException();
        }
    }
}
