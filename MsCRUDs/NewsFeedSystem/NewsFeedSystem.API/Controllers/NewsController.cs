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

namespace NewsFeedSystem.API.Controllers
{
    [ApiController]
    [Asp.Versioning.ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class NewsController : ControllerBase, INewsAPI
    {
        private readonly ILogger<NewsController> _logger;

        public NewsController(ILogger<NewsController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateResponseDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create(CreateNewsRequestDto request)
        {
            _logger.LogInformation(0, @"Create request {version:apiVersion}", 1);
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(uint newsId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> GetNewsPost(uint newsId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> GetHeadlines(uint? minNewsId, uint? maxNewsId = null)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IEnumerable<HeadLineDto>> GetHeadlinesByTag(uint tagId, uint minNewsId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IEnumerable<HeadLineDto>> GetHeadlinesByTopic(uint topicId, uint minNewsId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ProducesResponseType(typeof(UpdateResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(UpdateResult), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(UpdateNewsRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}
