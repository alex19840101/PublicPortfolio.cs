using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Auth;
using ShopServices.Core.Services;
using Warehouses.API.Contracts.Requests;
using Warehouses.API.Contracts.Responses;

namespace Warehouses.API.Controllers
{
    /// <summary> Контроллер управления данными складов </summary>
    [ApiController]
    [Asp.Versioning.ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class WarehousesController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;
        private readonly ILogger<WarehousesController> _logger;

        /// <summary> Конструктор контроллера управления данными складов </summary>
        public WarehousesController(
            IWarehouseService warehouseService,
            ILogger<WarehousesController> logger)
        {
            _warehouseService = warehouseService;
            _logger = logger;
        }

        /// <summary> Добавление склада </summary>
        /// <param name="addWarehouseRequestDto"> Запрос на добавление склада </param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = $"{Roles.Admin}, {Roles.Manager}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> AddWarehouse(AddWarehouseRequestDto addWarehouseRequestDto)
        {
            var createResult = await _warehouseService.AddWarehouse(WarehousesMapper.PrepareCoreWarehouse(addWarehouseRequestDto));

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
            _logger.LogInformation((EventId)(int)result!.Id!, @"added Warehouse {result.Id}", result!.Id!);

            return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
        }

        /// <summary> Получение информации о складе по id склада </summary>
        /// <param name="warehouseId"> id склада </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(WarehouseResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [Authorize(Roles = $"{Roles.Admin}, {Roles.Courier}, {Roles.Developer}, {Roles.Manager}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> GetWarehouseById(uint warehouseId)
        {
            var warehouse = await _warehouseService.GetWarehouseById(warehouseId);

            if (warehouse is null)
                return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

            return Ok(WarehousesMapper.GetWarehouseDto(warehouse));
        }

        /// <summary> Получение информации о складах </summary>
        /// <param name="regionCode"> Код города/населенного пункта </param>
        /// <param name="nameSubString"> Подстрока названия склада </param>
        /// <param name="addressSubString"> Подстрока - адрес </param>
        /// <param name="byPage"> Количество на странице </param>
        /// <param name="page"> Номер страницы </param>
        /// <param name="ignoreCase"> Игнорировать ли регистр символов </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<WarehouseResponseDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IEnumerable<WarehouseResponseDto>> GetWarehouses(
            uint? regionCode = null,
            string? nameSubString = null,
            string? addressSubString = null,
            [Range(1, 100)] uint byPage = 10,
            [Range(1, uint.MaxValue)] uint page = 1,
            bool ignoreCase = true)
        {
            var shopsCollection = await _warehouseService.GetWarehouses(
                regionCode: regionCode,
                nameSubString: nameSubString,
                addressSubString: addressSubString,
                byPage: byPage,
                page: page,
                ignoreCase: ignoreCase);

            if (!shopsCollection.Any())
                return [];

            return shopsCollection.GetWarehousesDtos();
        }

        /// <summary> Архивация (удаление) склада по id </summary>
        /// <param name="id"> id склада для архивации </param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [Authorize(Roles = $"{Roles.Admin}, {Roles.Developer}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> ArchiveWarehouse(uint id)
        {
            var deleteResult = await _warehouseService.ArchiveWarehouse(id);

            if (deleteResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(deleteResult);

            if (deleteResult.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(deleteResult) { StatusCode = StatusCodes.Status403Forbidden };

            return Ok(deleteResult);
        }

        /// <summary> Обновление информации о складе </summary>
        /// <param name="updateWarehouseRequestDto"> Информация о складе для обновления </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [Authorize(Roles = $"{Roles.Admin}, {Roles.Developer}, {Roles.Manager}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> UpdateWarehouse(UpdateWarehouseRequestDto updateWarehouseRequestDto)
        {
            var updateResult = await _warehouseService.UpdateWarehouse(WarehousesMapper.GetCoreWarehouse(updateWarehouseRequestDto));

            if (updateResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(updateResult);

            if (updateResult.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(updateResult) { StatusCode = StatusCodes.Status403Forbidden };

            return Ok(updateResult);
        }
    }
}
