using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shops.API.Contracts.Requests;
using Shops.API.Contracts.Responses;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Auth;
using ShopServices.Core.Services;

namespace Shops.API.Controllers
{
    /// <summary> Контроллер управления данными магазинов </summary>
    [ApiController]
    [Asp.Versioning.ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class ShopsController : ControllerBase
    {
        private readonly IShopsService _shopsService;
        private readonly ILogger<ShopsController> _logger;

        /// <summary> Конструктор контроллера управления данными магазинов </summary>
        public ShopsController(
            IShopsService shopsService,
            ILogger<ShopsController> logger)
        {
            _shopsService = shopsService;
            _logger = logger;
        }


        /// <summary> Добавление магазина </summary>
        /// <param name="addShopRequestDto"> Запрос на добавление магазина </param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = $"{Roles.Admin}, {Roles.Manager}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> AddShop(AddShopRequestDto addShopRequestDto)
        {
            var createResult = await _shopsService.AddShop(ShopsMapper.PrepareCoreShop(addShopRequestDto));

            if (createResult.StatusCode == HttpStatusCode.BadRequest)
                return new BadRequestObjectResult(new ProblemDetails { Title = createResult.Message });

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
            _logger.LogInformation((EventId)(int)result!.Id!, @"added Shop {result.Id}", result!.Id!);

            return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
        }

        /// <summary> Получение информации о магазине по id магазина </summary>
        /// <param name="shopId"> id магазина </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ShopResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetShopById(uint shopId)
        {
            var shop = await _shopsService.GetShopById(shopId);

            if (shop is null)
                return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

            return Ok(ShopsMapper.GetShopDto(shop));
        }

        /// <summary> Получение информации о магазинах </summary>
        /// <param name="regionCode"> Код города/населенного пункта </param>
        /// <param name="nameSubString"> Подстрока названия магазина </param>
        /// <param name="addressSubString"> Подстрока - адрес </param>
        /// <param name="byPage"> Количество на странице </param>
        /// <param name="page"> Номер страницы </param>
        /// <param name="ignoreCase"> Игнорировать ли регистр символов </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ShopResponseDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IEnumerable<ShopResponseDto>> GetShops(
            uint? regionCode = null,
            string? nameSubString = null,
            string? addressSubString = null,
            [Range(1, 100)] uint byPage = 10,
            [Range(1, uint.MaxValue)] uint page = 1,
            bool ignoreCase = true)
        {
            var shopsCollection = await _shopsService.GetShops(
                regionCode: regionCode,
                nameSubString: nameSubString,
                addressSubString: addressSubString,
                byPage: byPage,
                page: page,
                ignoreCase: ignoreCase);

            if (!shopsCollection.Any())
                return [];

            return shopsCollection.GetShopsDtos();
        }


        /// <summary> Архивация (удаление) магазина по id </summary>
        /// <param name="id"> id магазина для архивации </param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [Authorize(Roles = $"{Roles.Admin}, {Roles.Developer}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> ArchiveShop(uint id)
        {
            var deleteResult = await _shopsService.ArchiveShop(id);

            if (deleteResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(deleteResult);

            if (deleteResult.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(deleteResult) { StatusCode = StatusCodes.Status403Forbidden };

            return Ok(deleteResult);
        }

        /// <summary> Обновление информации о магазине </summary>
        /// <param name="updateShopRequestDto"> Информация о магазине для обновления </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [Authorize(Roles = $"{Roles.Admin}, {Roles.Developer}, {Roles.Manager}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> UpdateShop(UpdateShopRequestDto updateShopRequestDto)
        {
            var updateResult = await _shopsService.UpdateShop(ShopsMapper.GetCoreShop(updateShopRequestDto));

            if (updateResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(updateResult);

            if (updateResult.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(updateResult) { StatusCode = StatusCodes.Status403Forbidden };

            return Ok(updateResult);
        }
    }
}
