using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Deliveries.API.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Auth;
using ShopServices.Core.Services;

namespace Deliveries.API.Controllers
{
    /// <summary> ���������� ���������� ������� �� ��������� (��������) ������� </summary>
    [ApiController]
    [Asp.Versioning.ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;
        private readonly ILogger<DeliveryController> _logger;

        /// <summary> ����������� ����������� ���������� ������� �� ��������� (��������) ������� </summary>
        public DeliveryController(
            IDeliveryService deliveryService,
            ILogger<DeliveryController> logger)
        {
            _deliveryService = deliveryService;
            _logger = logger;
        }

        /// <summary> ���������� ��������� (��������) </summary>
        /// <param name="deliveryDto"> ������ �� ���������� ��������� (��������) </param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = Roles.Manager)]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> AddDelivery(DeliveryDto deliveryDto)
        {
            var createResult = await _deliveryService.AddDelivery(DeliveriesMapper.PrepareCoreDelivery(deliveryDto));

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
            _logger.LogInformation((EventId)(int)result!.Id!, @"added Delivery {result.Id}", result!.Id!);

            return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
        }

        /// <summary> ��������� ���������� �� ��������� (��������) �� id ��������� (��������) </summary>
        /// <param name="deliveryId"> id ��������� (��������) </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(DeliveryDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [Authorize(Roles = $"{Roles.Courier},{Roles.Manager}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> GetDeliveryById(uint deliveryId)
        {
            var delivery = await _deliveryService.GetDeliveryById(deliveryId);

            if (delivery is null)
                return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

            return Ok(DeliveriesMapper.GetDeliveryDto(delivery));
        }

        /// <summary> ��������� ���������� � ���������� (���������) </summary>
        /// <param name="regionCode"> (�����������) ��� ������/����������� ������ </param>
        /// <param name="buyerId"> (�����������) Id ���������� </param>
        /// <param name="addressSubString"> (�����������) ��������� - ����� </param>
        /// <param name="byPage"> ���������� �� �������� </param>
        /// <param name="page"> ����� �������� </param>
        /// <param name="ignoreCase"> ������������ �� ������� �������� </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DeliveryDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Roles = $"{Roles.Courier},{Roles.Manager}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IEnumerable<DeliveryDto>> GetDeliveries(
            uint? regionCode,
            uint? buyerId,
            string? addressSubString = null,
            [Range(1, 100)] uint byPage = 10,
            [Range(1, uint.MaxValue)] uint page = 1,
            bool ignoreCase = true)
        {
            var deliveriesCollection = await _deliveryService.GetDeliveries(
                regionCode: regionCode,
                buyerId: buyerId,
                addressSubString: addressSubString,
                byPage: byPage,
                page: page,
                ignoreCase: ignoreCase);

            if (!deliveriesCollection.Any())
                return [];

            return deliveriesCollection.GetDeliveriesDtos();
        }

        /// <summary> ��������� ���������� � ���������� (���������) ������ </summary>
        /// <param name="orderId"> Id ������ </param>
        /// <param name="byPage"> ���������� �� �������� </param>
        /// <param name="page"> ����� �������� </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DeliveryDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [Authorize(Roles = $"{Roles.Courier},{Roles.Manager}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IEnumerable<DeliveryDto>> GetDeliveriesForOrder(
            uint orderId,
            [Range(1, 100)] uint byPage = 10,
            [Range(1, uint.MaxValue)] uint page = 1)
        {
            var deliveriesCollection = await _deliveryService.GetDeliveriesForOrder(
                orderId: orderId,
                byPage: byPage,
                page: page);

            if (!deliveriesCollection.Any())
                return [];

            return deliveriesCollection.GetDeliveriesDtos();
        }

        /// <summary> ���������� ���������� � ��������� (��������) ������ </summary>
        /// <param name="updateRequestDto"> ���������� � ��������� (��������) ������ ��� ���������� </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [Authorize(Roles = $"{Roles.Manager},{Roles.Courier}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> UpdateDelivery(DeliveryDto updateRequestDto)
        {
            var updateResult = await _deliveryService.UpdateDelivery(DeliveriesMapper.GetCoreDelivery(updateRequestDto));

            if (updateResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(updateResult);

            if (updateResult.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(updateResult) { StatusCode = StatusCodes.Status403Forbidden };

            return Ok(updateResult);
        }



        /// <summary> ������ (��������� (��������)) ��������� (��������) ������ �� id ��������� </summary>
        /// <param name="deliveryId"> id ��������� </param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [Authorize(Roles = Roles.Manager)]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> ArchiveDelivery(uint deliveryId)
        {
            var deleteResult = await _deliveryService.ArchiveDelivery(deliveryId);

            if (deleteResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(deleteResult);

            if (deleteResult.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(deleteResult) { StatusCode = StatusCodes.Status403Forbidden };

            return Ok(deleteResult);
        }
    }
}
