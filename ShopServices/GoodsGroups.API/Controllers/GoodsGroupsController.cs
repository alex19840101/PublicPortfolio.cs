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
    /// <summary> ���������� ��� ������ � �������� (�����������) ������� </summary>
    [ApiController]
    [Route("[controller]")]
    public class GoodsGroupsController : ControllerBase
    {
        private readonly ILogger<GoodsGroupsController> _logger;
        private readonly IGoodsGroupsService _goodsGroupsService;

        public GoodsGroupsController(
            IGoodsGroupsService goodsGroupsService,
            ILogger<GoodsGroupsController> logger)
        {
            _goodsGroupsService = goodsGroupsService;
            _logger = logger;
        }

        /// <summary> ���������� ��������� </summary>
        /// <param name="categoryDto"> ������ �� ���������� ��������� </param>
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

        /// <summary> ��������� ��������� �� id </summary>
        /// <param name="id"> id ��������� </param>
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


        /// <summary> ��������� ���������� � ��������� </summary>
        /// <param name="articleSubString"> ��������� �������� ��������� </param>
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

        /// <summary> ��������� ���������� � ������� </summary>
        /// <param name="nameSubString"> ��������� �������� ������ </param>
        /// <param name="brand"> ����� (�������������) </param>
        /// <param name="byPage"> ���������� ������� �� �������� </param>
        /// <param name="page"> ����� �������� </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Category>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IEnumerable<Category>> GetCategorys(
            string nameSubString,
            string brand = null,
            uint byPage = 10,
            uint page = 1)
        {
            var categoriesCollection = await _goodsGroupsService.GetCategories(nameSubString, brand, byPage, page);

            if (!categoriesCollection.Any())
                return [];

            return categoriesCollection.GetCategoriesDtos();
        }

        /// <summary> ���������� ���������� � ������ </summary>
        /// <param name="categoryDto"> ���������� � ������ ��� ���������� </param>
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

        /// <summary> �������� (������� ��������) ������ �� id </summary>
        /// <param name="id"> id ������ ��� �������� (���������) </param>
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
