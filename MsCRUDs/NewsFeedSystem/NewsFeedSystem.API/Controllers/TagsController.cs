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
using NewsFeedSystem.Core.Services;
using Microsoft.AspNetCore.Http;
using NewsFeedSystem.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using NewsFeedSystem.Core;
using System.Linq;

namespace NewsFeedSystem.API.Controllers
{
    /// <summary> Контроллер управления тегами </summary>
    [ApiController]
    [Asp.Versioning.ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class TagsController : ControllerBase, ITagsAPI
    {
        private readonly ITagsService _tagsService;
        private readonly ILogger<TagsController> _logger;

        /// <summary> Конструктор контроллера управления тегами </summary>
        public TagsController(ITagsService tagsService,
            ILogger<TagsController> logger)
        {
            _tagsService = tagsService;
            _logger = logger;
        }

        /// <summary>
        /// Создание тега
        /// </summary>
        /// <param name="createTagRequestDto"> Запрос на создание тега </param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(CreateResponseDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create(CreateTagRequestDto createTagRequestDto)
        {
            var createResult = await _tagsService.Create(new Core.Tag(id: 0, name: createTagRequestDto.Tag ));

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
            _logger.LogInformation((EventId)(int)result!.Id!, @"added Tag {result.Id}", result!.Id!);
            return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
        }


        /// <summary>
        /// Удаление тега
        /// </summary>
        /// <param name="tagId"> Id тега </param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(DeleteResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(DeleteResult), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(DeleteResult), (int)HttpStatusCode.NotFound)]
        [Authorize(Roles = "admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Delete(uint tagId)
        {
            var deleteResult = await _tagsService.Delete(tagId);

            if (deleteResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(deleteResult);
            if (deleteResult.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(deleteResult) { StatusCode = StatusCodes.Status403Forbidden };

            return Ok(deleteResult);
        }

        /// <summary>
        /// Получение тега
        /// </summary>
        /// <param name="tagId"> Id тега </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(TagDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(MessageResponseDto), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(uint tagId)
        {
            var tag = await _tagsService.Get(tagId);

            if (tag is null)
                return NotFound(new MessageResponseDto { Message = ErrorStrings.NEWS_NOT_FOUND });

            return Ok(tag.GetTagDto());
        }

        /// <summary>
        /// Получение тегов
        /// </summary>
        /// <param name="minTagId"> Минимальный Id тега </param>
        /// <param name="maxTagId"> Максимальный Id тега </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TagDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IEnumerable<TagDto>> GetTags(uint? minTagId, uint? maxTagId = null)
        {
            var tagsCollection = await _tagsService.GetTags(minTagId, maxTagId);

            if (!tagsCollection.Any())
                return [];

            return tagsCollection.GetTagDtos();
        }

        /// <summary>
        /// Обновление тега
        /// </summary>
        /// <param name="tagDto"> Запрос на обновление тега </param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(UpdateResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(UpdateResult), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update(TagDto tagDto)
        {
            var updateResult = await _tagsService.Update(tagDto.GetTag());

            if (updateResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(updateResult);

            return Ok(updateResult);
        }
    }
}
