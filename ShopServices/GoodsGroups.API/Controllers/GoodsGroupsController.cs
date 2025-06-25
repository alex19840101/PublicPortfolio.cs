using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GoodsGroups.API.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Auth;
using ShopServices.Core.Services;

namespace GoodsGroups.API.Controllers
{
    /// <summary> Контроллер для работы с группами (категориями) товаров </summary>
    [ApiController]
    [Asp.Versioning.ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class GoodsGroupsController : ControllerBase
    {
        private readonly ILogger<GoodsGroupsController> _logger;
        private readonly IGoodsGroupsService _goodsGroupsService;

        /// <summary> Конструктор контроллера управления группами (категориями) товаров </summary>
        public GoodsGroupsController(
            IGoodsGroupsService goodsGroupsService,
            ILogger<GoodsGroupsController> logger)
        {
            _goodsGroupsService = goodsGroupsService;
            _logger = logger;
        }

        /// <summary> Добавление категории </summary>
        /// <param name="categoryDto"> Запрос на добавление категории </param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = $"{Roles.Admin}, {Roles.Developer}, {Roles.Manager}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> AddCategory(Category categoryDto)
        {
            var createResult = await _goodsGroupsService.AddCategory(CategoryMapper.GetCoreCategory(categoryDto));

            if (createResult.StatusCode == HttpStatusCode.BadRequest)
                return new BadRequestObjectResult(new ProblemDetails { Title = createResult.Message });

            if (createResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new Result
                {
                    Message = createResult.Message,
                    StatusCode = createResult.StatusCode
                });

            if (createResult.StatusCode == HttpStatusCode.Conflict)
                return new ConflictObjectResult(new Result
                {
                    Message = createResult.Message,
                    StatusCode = createResult.StatusCode
                });

            if (createResult.StatusCode != HttpStatusCode.Created)
                return new ObjectResult(new Result { Message = createResult.Message }) { StatusCode = StatusCodes.Status500InternalServerError };

            var result = new Result
            {
                Id = createResult!.Id!.Value,
                Message = createResult.Message
            };
            _logger.LogInformation((EventId)(int)result!.Id!, @"added Category {result.Id}", result!.Id!);

            return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };

        }

        /// <summary> Получение категории по id </summary>
        /// <param name="id"> id категории </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Category), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCategoryById(uint id)
        {
            var category = await _goodsGroupsService.GetCategoryById(id);

            if (category is null)
                return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

            return Ok(CategoryMapper.GetCategoryDto(category));
        }


        /// <summary> Получение информации о категории </summary>
        /// <param name="nameSubString"> Название категории </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Category), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCategoryByName(string nameSubString)
        {
            var category = await _goodsGroupsService.GetCategoryByName(nameSubString);

            if (category is null)
                return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

            return Ok(CategoryMapper.GetCategoryDto(category));
        }

        /// <summary> Получение информации о категориях </summary>
        /// <param name="nameSubString"> Подстрока названия категории </param>
        /// <param name="brand"> Подстрока - бренд (производитель) </param>
        /// <param name="byPage"> Количество товаров на странице </param>
        /// <param name="page"> Номер страницы </param>
        /// <param name="ignoreCase"> Игнорировать ли регистр символов </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Category>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IEnumerable<Category>> GetCategories(
            string? nameSubString = null,
            string? brand = null,
            [Range(1,100)] uint byPage = 10,
            [Range(1, uint.MaxValue)] uint page = 1,
            bool ignoreCase = true)
        {
            var categoriesCollection = await _goodsGroupsService.GetCategories(
                nameSubString: nameSubString,
                brand: brand,
                byPage: byPage,
                page: page,
                ignoreCase: ignoreCase);

            if (!categoriesCollection.Any())
                return [];

            return categoriesCollection.GetCategoriesDtos();
        }

        /// <summary> Обновление информации о категории </summary>
        /// <param name="categoryDto"> Информация о категории для обновления </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [Authorize(Roles = $"{Roles.Admin}, {Roles.Developer}, {Roles.Manager}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> UpdateCategory(Category categoryDto)
        {
            var updateResult = await _goodsGroupsService.UpdateCategory(CategoryMapper.GetCoreCategory(categoryDto));

            if (updateResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(updateResult);

            return Ok(updateResult);
        }

        /// <summary> Архивация категории по id </summary>
        /// <param name="id"> id категории для архивации </param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [Authorize(Roles = $"{Roles.Admin}, {Roles.Developer}, {Roles.Manager}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> ArchiveCategory(uint id)
        {
            var deleteResult = await _goodsGroupsService.ArchiveCategory(id);

            if (deleteResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(deleteResult);

            if (deleteResult.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(deleteResult) { StatusCode = StatusCodes.Status403Forbidden };

            return Ok(deleteResult);
        }

        /// <summary> Удаление категории по id </summary>
        /// <param name="id"> id категории для удаления </param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [Authorize(Roles = $"{Roles.Admin}, {Roles.Developer}, {Roles.Manager}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> DeleteCategory(uint id)
        {
            var deleteResult = await _goodsGroupsService.DeleteCategory(id);

            if (deleteResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(deleteResult);

            if (deleteResult.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(deleteResult) { StatusCode = StatusCodes.Status403Forbidden };

            return Ok(deleteResult);
        }
    }
}
