using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsFeedSystem.Contracts.Interfaces;
using NewsFeedSystem.Contracts.Requests;

namespace NewsFeedSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : ControllerBase, INewsAPI
    {
        private readonly ILogger<NewsController> _logger;

        public NewsController(ILogger<NewsController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
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
        public async Task<IActionResult> Update(UpdateNewsRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}
