using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsFeedSystem.API.Contracts.Interfaces;
using NewsFeedSystem.API.Contracts.Requests;
using NewsFeedSystem.API.Contracts.Responses;
using NewsFeedSystem.API.Extensions;
using NewsFeedSystem.Core;
using NewsFeedSystem.Core.Results;
using NewsFeedSystem.Core.Services;

namespace NewsFeedSystem.API.Controllers
{
    /// <summary> Контроллер управления новостными постами </summary>
    [ApiController]
    [Asp.Versioning.ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class NewsController : ControllerBase, INewsAPI
    {
        private readonly ILogger<NewsController> _logger;
        private readonly INewsService _newsService;

        /// <summary> Конструктор контроллера управления новостными постами </summary>
        public NewsController(INewsService newsService,
            ILogger<NewsController> logger)
        {
            _newsService = newsService;
            _logger = logger;
        }

        /// <summary>
        /// Создание новостного поста
        /// </summary>
        /// <param name="createNewsRequestDto"> Запрос на создание новостного поста </param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(CreateResponseDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create(CreateNewsRequestDto createNewsRequestDto)
        {
            var createResult = await _newsService.Create(createNewsRequestDto.GetNewsPostForCreate());

            if (createResult.StatusCode == HttpStatusCode.BadRequest)
                return new BadRequestObjectResult(new ProblemDetails { Title = createResult.Message });

            if (createResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new MessageResponseDto { Message = createResult.Message });

            if (createResult.StatusCode == HttpStatusCode.Conflict)
                return new ConflictObjectResult(new MessageResponseDto { Message = createResult.Message });

            var result = new CreateResponseDto
            {
                Id = createResult!.Id!.Value,
                Message = createResult.Message
            };

            _logger.LogInformation((EventId)(int)result!.Id!, @"added NewsPost {result.Id}", result!.Id!);

            return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
        }

        /// <summary> Удаление новостного поста </summary>
        [HttpDelete]
        [ProducesResponseType(typeof(DeleteResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(DeleteResult), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(DeleteResult), (int)HttpStatusCode.NotFound)]
        [Authorize(Roles = "admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Delete(uint newsId)
        {
            var deleteResult = await _newsService.DeleteNewsPost(newsId);

            if (deleteResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(deleteResult);
            if (deleteResult.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(deleteResult) { StatusCode = StatusCodes.Status403Forbidden };

            return Ok(deleteResult);
        }

        /// <summary> Получение новостного поста </summary>
        [HttpGet]
        [ProducesResponseType(typeof(NewsPostDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(MessageResponseDto), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetNewsPost(uint newsId)
        {
            var newsPost = await _newsService.Get(newsId);

            if (newsPost is null)
                return NotFound(new MessageResponseDto { Message = ErrorStrings.NEWS_NOT_FOUND });

            return Ok(newsPost.GetNewsPostDto());
        }

        /// <summary>
        /// Получение заголовков новостных постов
        /// </summary>
        /// <param name="minNewsId"> Минимальный Id новостного поста </param>
        /// <param name="maxNewsId"> Максимальный Id новостного поста </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<HeadLineDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IEnumerable<HeadLineDto>> GetHeadlines(uint? minNewsId, uint? maxNewsId = null)
        {
            var headlinesCollection = await _newsService.GetHeadlines(minNewsId, maxNewsId);

            if (!headlinesCollection.Any())
                return [];

            return headlinesCollection.GetHeadlineDtos();
        }

        /// <summary>
        /// Получение заголовков новостных постов
        /// </summary>
        /// <param name="tagId"> Id тега </param>
        /// <param name="minNewsId"> Минимальный Id новостного поста </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<HeadLineDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IEnumerable<HeadLineDto>> GetHeadlinesByTag(uint tagId, uint minNewsId)
        {
            var headlinesCollection = await _newsService.GetHeadlinesByTag(tagId, minNewsId);

            if (!headlinesCollection.Any())
                return [];

            return headlinesCollection.GetHeadlineDtos();
        }

        /// <summary>
        /// Получение заголовков новостных постов по Id темы
        /// </summary>
        /// <param name="topicId"> Id темы </param>
        /// <param name="minNewsId"> Минимальный Id новостного поста </param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<HeadLineDto>> GetHeadlinesByTopic(uint topicId, uint minNewsId)
        {
            var headlinesCollection = await _newsService.GetHeadlinesByTopic(topicId, minNewsId);

            if (!headlinesCollection.Any())
                return [];

            return headlinesCollection.GetHeadlineDtos();
        }

        /// <summary>
        /// Обновление новостного поста
        /// </summary>
        /// <param name="request"> Запрос на обновление новостного поста </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(UpdateResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(UpdateResult), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(UpdateNewsRequestDto request)
        {
            var updateResult = await _newsService.Update(request.GetNewsPostForUpdate());

            if (updateResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(updateResult);

            return Ok(updateResult);
        }
    }
}
