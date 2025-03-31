using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsFeedSystem.API.Contracts;
using NewsFeedSystem.API.Contracts.Interfaces;
using NewsFeedSystem.API.Contracts.Requests;
using NewsFeedSystem.API.Contracts.Responses;
using NewsFeedSystem.API.Extensions;
using NewsFeedSystem.Core;
using NewsFeedSystem.Core.Results;
using NewsFeedSystem.Core.Services;

namespace NewsFeedSystem.API.Controllers
{
    /// <summary> Контроллер управления темами </summary>
    [ApiController]
    [Asp.Versioning.ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class TopicsController : ControllerBase, ITopicsAPI
    {
        private readonly ITopicsService _topicsService;
        private readonly ILogger<TopicsController> _logger;

        /// <summary> Конструктор контроллера управления темами </summary>
        public TopicsController(ITopicsService topicsService,
            ILogger<TopicsController> logger)
        {
            _topicsService = topicsService;
            _logger = logger;
        }


        /// <summary>
        /// Создание новостной темы
        /// </summary>
        /// <param name="createTopicRequestDto"> Запрос на создание темы </param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(CreateResponseDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create(CreateTopicRequestDto createTopicRequestDto)
        {
            var createResult = await _topicsService.Create(new Core.Topic(id: 0, name: createTopicRequestDto.Topic));

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
            _logger.LogInformation((EventId)(int)result!.Id!, @"added Topic {result.Id}", result!.Id!);
            return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
        }

        
        /// <summary> Получение темы </summary>
        /// <param name="topicId"> Id темы </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(TopicDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(MessageResponseDto), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(uint topicId)
        {
            var topic = await _topicsService.Get(topicId);

            if (topic is null)
                return NotFound(new MessageResponseDto { Message = ErrorStrings.NEWS_NOT_FOUND });

            return Ok(topic.GetTopicDto());
        }

        /// <summary>
        ///  Получение тем
        /// </summary>
        /// <param name="minTopicId"> Минимальный Id темы </param>
        /// <param name="maxTopicId"> Максимальный Id темы </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(TopicDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IEnumerable<TopicDto>> GetTopics(uint? minTopicId, uint? maxTopicId = null)
        {
            var topicsCollection = await _topicsService.GetTopics(minTopicId, maxTopicId);

            if (!topicsCollection.Any())
                return [];

            return topicsCollection.GetTopicDtos();
        }

        /// <summary>
        /// Обновление темы
        /// </summary>
        /// <param name="topicDto"> Запрос на обновление темы </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(UpdateResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(UpdateResult), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(TopicDto topicDto)
        {
            var updateResult = await _topicsService.Update(topicDto.GetTopic());

            if (updateResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(updateResult);

            return Ok(updateResult);
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
        [Authorize(Roles = "admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Delete(uint topicId)
        {
            var deleteResult = await _topicsService.Delete(topicId);

            if (deleteResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(deleteResult);
            if (deleteResult.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(deleteResult) { StatusCode = StatusCodes.Status403Forbidden };

            return Ok(deleteResult);
        }
    }
}
