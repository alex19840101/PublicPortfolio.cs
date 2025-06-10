using System.Collections.Generic;
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
using ShopServices.Core.Services;

namespace GoodsGroups.API.Controllers
{
    /// <summary> Контроллер для работы с группами (категориями) товаров </summary>
    [ApiController]
    [Route("[controller]")]
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
        [Authorize(Roles = "admin, developer, manager")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddCategory(Category categoryDto)
        {
            var createResult = await _goodsGroupsService.AddCategory(CategoryMapper.GetCoreCategory(categoryDto));

            if (createResult.StatusCode == HttpStatusCode.BadRequest)
                return new BadRequestObjectResult(new ProblemDetails { Title = createResult.Message });

            if (createResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(new Result { Message = createResult.Message });

            if (createResult.StatusCode == HttpStatusCode.Conflict)
                return new ConflictObjectResult(new Result { Message = createResult.Message });

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
        /// <param name="nameSubString"> Подстрока названия категории </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Category>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IEnumerable<Category>> GetCategoryByName(string nameSubString)
        {
            var categoriesCollection = await _goodsGroupsService.GetCategoryByName(nameSubString);

            if (!categoriesCollection.Any())
                return [];

            return categoriesCollection.GetCategoriesDtos();
        }

        /// <summary> Получение информации о категориях </summary>
        /// <param name="nameSubString"> Подстрока названия категории </param>
        /// <param name="brand"> Бренд (производитель) </param>
        /// <param name="byPage"> Количество товаров на странице </param>
        /// <param name="page"> Номер страницы </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Category>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IEnumerable<Category>> GetCategories(
            string nameSubString,
            string? brand = null,
            uint byPage = 10,
            uint page = 1)
        {
            var categoriesCollection = await _goodsGroupsService.GetCategories(nameSubString, brand, byPage, page);

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
        [Authorize(Roles = "admin, developer, manager")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateCategory(Category categoryDto)
        {
            var updateResult = await _goodsGroupsService.UpdateCategory(CategoryMapper.GetCoreCategory(categoryDto));

            if (updateResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(updateResult);

            return Ok(updateResult);
        }

        /// <summary> Удаление (пометка архивным) категории по id </summary>
        /// <param name="id"> id категории для удаления (архивации) </param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [Authorize(Roles = "admin, developer, manager")]
        [Authorize(AuthenticationSchemes = "Bearer")]
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
