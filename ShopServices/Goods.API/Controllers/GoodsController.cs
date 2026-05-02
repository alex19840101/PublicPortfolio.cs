using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Goods.API.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Auth;
using ShopServices.Core.Services;

namespace Goods.API.Controllers
{
    /// <summary> Контроллер управления данными по товарам </summary>
    [ApiController]
    [Asp.Versioning.ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class GoodsController : ControllerBase
    {
        private readonly IGoodsService _goodsService;
        private readonly ILogger<GoodsController> _logger;

        /// <summary> Конструктор контроллера управления данными по товарам </summary>
        public GoodsController(
            IGoodsService goodsService,
            ILogger<GoodsController> logger)
        {
            _goodsService = goodsService;
            _logger = logger;
        }

        /// <summary> Добавление товара </summary>
        /// <param name="productDto"> Запрос на добавление товара </param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = $"{Roles.Admin}, {Roles.Developer}, {Roles.Manager}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> AddProduct(Product productDto)
        {
            var createResult = await _goodsService.AddProduct(ProductMapper.GetCoreProduct(productDto));

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
            _logger.LogInformation((EventId)(int)result!.Id!, @"added Product {result.Id}", result!.Id!);
            
            return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
        }

        /// <summary> Получение товара по id </summary>
        /// <param name="id"> id товара </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProductById(uint id)
        {
            var product = await _goodsService.GetProductById(id);

            if (product is null)
                return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

            return Ok(ProductMapper.GetProductDto(product));
        }


        /// <summary> Получение информации о товарах </summary>
        /// <param name="articleSubString"> Подстрока артикула производителя </param>
        /// <param name="brand"> Подстрока - бренд (производитель) </param>
        /// <param name="byPage"> Количество товаров на странице </param>
        /// <param name="page"> Номер страницы </param>
        /// <param name="ignoreCase"> Игнорировать ли регистр символов </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IEnumerable<Product>> GetProductsByArticle(
            string articleSubString,
            string? brand = null,
            [Range(1, 100)] uint byPage = 10,
            [Range(1, uint.MaxValue)] uint page = 1,
            bool ignoreCase = true)
        {
            var productsCollection = await _goodsService.GetProductsByArticle(
                articleSubString: articleSubString,
                brand: brand,
                byPage: byPage,
                page: page,
                ignoreCase: ignoreCase);

            if (!productsCollection.Any())
                return [];

            return productsCollection.GetProductDtos();
        }

        /// <summary> Получение перечня товаров заданной категории </summary>
        /// <param name="category"> id категории товаров </param>
        /// <param name="paramsSubString"> Подстрока параметров товара </param>
        /// <param name="brand"> Подстрока - бренд (производитель) </param>
        /// <param name="byPage"> Количество товаров на странице </param>
        /// <param name="page"> Номер страницы </param>
        /// <param name="ignoreCase"> Игнорировать ли регистр символов </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IEnumerable<Product>> GetProductsByCategory(
            uint category,
            string? paramsSubString = null,
            string? brand = null,
            [Range(1, 100)] uint byPage = 10,
            [Range(1, uint.MaxValue)] uint page = 1,
            bool ignoreCase = true)
        {
            var productsCollection = await _goodsService.GetProductsByCategory(
                category: category,
                paramsSubString: paramsSubString,
                brand: brand,
                byPage: byPage,
                page: page,
                ignoreCase: ignoreCase);

            if (!productsCollection.Any())
                return [];

            return productsCollection.GetProductDtos();
        }

        /// <summary> Получение информации о товарах </summary>
        /// <param name="nameSubString"> Подстрока названия товара </param>
        /// <param name="brand"> Подстрока - бренд (производитель) </param>
        /// <param name="byPage"> Количество товаров на странице </param>
        /// <param name="page"> Номер страницы </param>
        /// <param name="ignoreCase"> Игнорировать ли регистр символов </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IEnumerable<Product>> GetProducts(
            string? nameSubString = null,
            string? brand = null,
            [Range(1, 100)] uint byPage = 10,
            [Range(1, uint.MaxValue)] uint page = 1,
            bool ignoreCase = true)
        {
            var productsCollection = await _goodsService.GetProducts(
                nameSubString: nameSubString,
                brand: brand,
                byPage: byPage,
                page: page,
                ignoreCase: ignoreCase);

            if (!productsCollection.Any())
                return [];

            return productsCollection.GetProductDtos();
        }

        /// <summary> Обновление информации о товаре </summary>
        /// <param name="productDto"> Информация о товаре для обновления </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [Authorize(Roles = $"{Roles.Admin}, {Roles.Developer}, {Roles.Manager}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> UpdateProduct(Product productDto)
        {
            var updateResult = await _goodsService.UpdateProduct(ProductMapper.GetCoreProduct(productDto));

            if (updateResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(updateResult);

            return Ok(updateResult);
        }

        /// <summary> Архивация товара по id </summary>
        /// <param name="id"> id товара для архивации </param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [Authorize(Roles = $"{Roles.Admin}, {Roles.Developer}, {Roles.Manager}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> ArchiveProduct(uint id)
        {
            var deleteResult = await _goodsService.ArchiveProduct(id);

            if (deleteResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(deleteResult);

            if (deleteResult.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(deleteResult) { StatusCode = StatusCodes.Status403Forbidden };

            return Ok(deleteResult);
        }
    }
}
