using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Prices.API.Contracts.Requests;
using Prices.API.Contracts.Responses;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Services;

namespace Prices.API.Controllers
{
    /// <summary> ���������� ���������� ������ ������� </summary>
    [ApiController]
    [Asp.Versioning.ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class PricesController : ControllerBase
    {
        private readonly IPricesService _priceService;
        private readonly ILogger<PricesController> _logger;

        /// <summary> ����������� ����������� ���������� ������ ������� </summary>
        public PricesController(
            IPricesService pricesService,
            ILogger<PricesController> logger)
        {
            _priceService = pricesService;
            _logger = logger;
        }


        /// <summary> ���������� ������� �� ����� </summary>
        /// <param name="priceDto"> ������ �� ���������� ������� �� ����� </param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Conflict)]
        [Authorize(Roles = "admin, developer, manager")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddPrice(AddPriceRequest priceDto)
        {
            var createResult = await _priceService.AddPrice(PriceMapper.GetCorePrice(priceDto));

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

            var result = new Result
            {
                Id = createResult!.Id!.Value,
                Message = createResult.Message
            };
            _logger.LogInformation((EventId)(int)result!.Id!, @"added Price {result.Id}", result!.Id!);

            return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
        }

        /// <summary> ��������� ������� �� ����� �� id ������� �� ����� </summary>
        /// <param name="priceId"> id ������� �� ����� </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PriceResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPriceById(uint priceId)
        {
            var price = await _priceService.GetPriceById(priceId);

            if (price is null)
                return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

            return Ok(PriceMapper.GetPriceDto(price));
        }


        /// <summary> ��������� ��� �� ����� ��� ���������� ���������� ��������� </summary>
        /// <param name="productId"> id ������ </param>
        /// <param name="actualFromDt"> ��������� �� ������ ������� </param>
        /// <param name="actualToDt"> ��������� �� ������ �������</param>
        /// <param name="byPage"> ���������� ������� �� �������� </param>
        /// <param name="page"> ����� �������� </param>
        /// <returns> IEnumerable(PriceResponseDto) - �������� ��� �� ����� ��� ���������� ���������� ��������� </returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PriceResponseDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IEnumerable<PriceResponseDto>> GetPricesForProduct(
            uint productId,
            DateTime actualFromDt,
            DateTime? actualToDt,
            [Range(1, 100)] uint byPage = 10,
            [Range(1, uint.MaxValue)] uint page = 1)
        {
            var pricesCollection = await _priceService.GetPricesForProduct(
                productId: productId,
                actualFromDt: actualFromDt,
                actualToDt: actualToDt,
                byPage: byPage,
                page: page);

            if (!pricesCollection.Any())
                return [];

            return pricesCollection.GetPriceDtos();
        }


        /// <summary> ��������� �� ����� ������� � ������ </summary>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotImplemented)]
        [Authorize(Roles = "admin, developer, manager")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult ChangePriceForProduct()
        {
            return new ObjectResult("Use Goods.API/UpdateProduct: (priceId, pricePerUnit)") { StatusCode = StatusCodes.Status501NotImplemented };
        }

        /// <summary> ��������� �� ������ ������� � ������ (��������� ������������� ������ ��� ������ / �������) </summary>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotImplemented)]
        [Authorize(Roles = "admin, developer, manager")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult ResetPriceForProduct()
        {
            return new ObjectResult("Use Goods.API/UpdateProduct: (priceId = null, pricePerUnit = null)") { StatusCode = StatusCodes.Status501NotImplemented };
        }


        /// <summary> ���������� ActualToDt ������� �� ����� </summary>
        /// <param name="updateActualToDtRequest"> ������ �� ���������� ActualToDt ������� �� ����� </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [Authorize(Roles = "admin, developer, manager")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateActualToDt(UpdateActualToDtRequest updateActualToDtRequest)
        {
            var updateResult = await _priceService.UpdateActualToDt(
                priceId: updateActualToDtRequest.PriceId,
                actualToDt: updateActualToDtRequest.ActualToDt);

            if (updateResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(updateResult);

            return Ok(updateResult);
        }
    }
}
