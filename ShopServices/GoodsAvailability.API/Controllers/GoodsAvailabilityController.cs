using System.ComponentModel.DataAnnotations;
using System.Net;
using GoodsAvailability.API.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Auth;
using ShopServices.Core.Services;

namespace GoodsAvailability.API.Controllers
{
    /// <summary>  онструктор контроллера контрол€ и учета наличи€ товаров </summary>
    [ApiController]
    [Asp.Versioning.ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class GoodsAvailabilityController : ControllerBase
    {
        private readonly IGoodsAvailabilityService _goodsAvailabilityService;
        private readonly ILogger<GoodsAvailabilityController> _logger;

        /// <summary>  онструктор контроллера контрол€ и учета наличи€ товаров </summary>
        public GoodsAvailabilityController(
            IGoodsAvailabilityService goodsAvailabilityService,
            ILogger<GoodsAvailabilityController> logger)
        {
            _goodsAvailabilityService = goodsAvailabilityService;
            _logger = logger;
        }

        /// <summary> ƒобавление записи о наличии товара в магазине/на складе </summary>
        /// <param name="availabilityDto"> «апрос на добавление записи о наличии товара в магазине/на складе </param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = $"{Roles.Admin}, {Roles.Developer}, {Roles.Manager}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> AddAvailability(Availability availabilityDto)
        {
            var createResult = await _goodsAvailabilityService.AddAvailability(AvailabilityMapper.GetCoreAvailability(availabilityDto));

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
            _logger.LogInformation((EventId)(int)result!.Id!, @"added Availability {result.Id}", result!.Id!);

            return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
        }

        /// <summary> ѕолучение записи о наличии товара в магазине/на складе по id записи </summary>
        /// <param name="id"> id записи о наличии товара в магазине/на складе </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Availability), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAvailabilityById(uint id)
        {
            var product = await _goodsAvailabilityService.GetAvailabilityById(id);

            if (product is null)
                return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

            return Ok(AvailabilityMapper.GetAvailabilityDto(product));
        }

        /// <summary> ѕолучение перечн€ записей о наличии товара в магазине/на складе по id товара </summary>
        /// <param name="productId"> id товара </param>
        /// <param name="byPage">  оличество записей на странице </param>
        /// <param name="page"> Ќомер страницы </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Availability>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IEnumerable<Availability>> GetAvailabilitiesByProductId(
            uint productId,
            [Range(1, 100)] uint byPage = 10,
            [Range(1, uint.MaxValue)] uint page = 1)
        {
            var availabilitiesCollection = await _goodsAvailabilityService.GetAvailabilitiesByProductId(
                productId: productId,
                byPage: byPage,
                page: page);

            if (!availabilitiesCollection.Any())
                return [];

            return availabilitiesCollection.GetAvailabilityDtos();
        }

        /// <summary> ќбновление информации о наличии товара в магазине/на складе </summary>
        /// <param name="availabilityDto"> »нформаци€ о наличии товара в магазине/на складе дл€ обновлени€ </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [Authorize(Roles = $"{Roles.Admin}, {Roles.Developer}, {Roles.Manager}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> UpdateAvailability(Availability availabilityDto)
        {
            var updateResult = await _goodsAvailabilityService.UpdateAvailability(AvailabilityMapper.GetCoreAvailability(availabilityDto));

            if (updateResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(updateResult);

            return Ok(updateResult);
        }

        /// <summary> ”даление информации о наличии товара в магазине/на складе по id записи о наличии </summary>
        /// <param name="id"> id записи о наличии </param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [Authorize(Roles = $"{Roles.Admin}, {Roles.Developer}, {Roles.Manager}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> DeleteAvailability(uint availabilityId)
        {
            var deleteResult = await _goodsAvailabilityService.DeleteAvailability(availabilityId);

            if (deleteResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(deleteResult);

            if (deleteResult.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(deleteResult) { StatusCode = StatusCodes.Status403Forbidden };

            return Ok(deleteResult);
        }
    }
}
