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
    /// <summary> ���������� ���������� ���������� ������� </summary>
    [ApiController]
    [Asp.Versioning.ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class NewsController : ControllerBase, INewsAPI
    {
        private readonly ILogger<NewsController> _logger;
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService,
            ILogger<NewsController> logger)
        {
            _newsService = newsService;
            _logger = logger;
        }

        /// <summary>
        /// �������� ���������� �����
        /// </summary>
        /// <param name="createTagRequestDto"> ������ �� �������� ���������� �����</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(CreateResponseDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create(CreateNewsRequestDto createTagRequestDto)
        {
            _logger.LogInformation(0, @"Create request {version:apiVersion}", 1);

            var createResult = await _newsService.Create(createTagRequestDto.GetNewsPostForCreate());

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
            return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
        }

        /// <summary> �������� ���������� ����� </summary>
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

        /// <summary> ��������� ���������� ����� </summary>
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
        /// ��������� ���������� ��������� ������
        /// </summary>
        /// <param name="minNewsId"> ����������� Id ���������� ����� </param>
        /// <param name="maxNewsId"> ������������ Id ���������� ����� </param>
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
        /// ��������� ���������� ��������� ������
        /// </summary>
        /// <param name="tagId"> Id ���� </param>
        /// <param name="minNewsId"> ����������� Id ���������� ����� </param>
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
        /// ��������� ���������� ��������� ������ �� Id ����
        /// </summary>
        /// <param name="topicId"> Id ���� </param>
        /// <param name="minNewsId"> ����������� Id ���������� ����� </param>
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
        /// ���������� ���������� �����
        /// </summary>
        /// <param name="request"> ������ �� ���������� ���������� ����� </param>
        /// <returns></returns>
        [HttpPost]
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
