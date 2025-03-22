using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsFeedSystem.API.Contracts.Responses;
using NewsFeedSystem.Contracts.Interfaces;
using NewsFeedSystem.Contracts.Requests;
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
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int newsId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> Read(int newsId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> ReadHeadlines(int? maxNewsId, int? minNewsId)
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
