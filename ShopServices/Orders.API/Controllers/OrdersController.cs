using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orders.API.Contracts.Requests;
using Orders.API.Contracts.Responses;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Services;

namespace Orders.API.Controllers
{
    /// <summary> Контроллер управления заказами </summary>
    [ApiController]
    [Asp.Versioning.ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;
        private readonly ILogger<OrdersController> _logger;

        /// <summary> Конструктор контроллера управления заказами </summary>
        public OrdersController(
            IOrdersService ordersService,
            ILogger<OrdersController> logger)
        {
            _ordersService = ordersService;
            _logger = logger;
        }

        /// <summary> Добавление заказа </summary>
        /// <param name="addOrderRequestDto"> Запрос на добавление заказа </param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = "buyer")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddOrder(AddOrderRequest addOrderRequestDto)
        {
            var createResult = await _ordersService.AddOrder(OrdersMapper.PrepareCoreOrder(addOrderRequestDto, httpContext: HttpContext));

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
            _logger.LogInformation((EventId)(int)result!.Id!, @"added Order {result.Id}", result!.Id!);

            return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
        }

        /// <summary> Получение информации о заказе по id заказа </summary>
        /// <param name="orderId"> id заказа </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(OrderResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [Authorize(Roles = "buyer, manager, courier")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOrderInfoById(uint orderId)
        {
            uint? buyerId = GetBuyerIdFromClaim();
            var order = await _ordersService.GetOrderInfoById(orderId, buyerId);

            if (order is null)
                return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

            return Ok(OrdersMapper.GetOrderResponseDto(order));
        }

        
        /// <summary> Отмена заказа покупателем </summary>
        /// <param name="cancelOrderRequest"> Запрос на отмену заказа покупателем </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = "buyer")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CancelOrderByBuyer(CancelOrderRequest cancelOrderRequest)
        {
            uint? buyerId = GetBuyerIdFromClaim();

            if (buyerId == null)
                return new ObjectResult(null) { StatusCode = StatusCodes.Status403Forbidden };

            var cancelResult = await _ordersService.CancelOrderByBuyer(
                buyerIdFromClaim: buyerId.Value,
                buyerIdFromRequest: cancelOrderRequest.BuyerId,
                orderId: cancelOrderRequest.OrderId,
                confirmationString: cancelOrderRequest.ConfirmationString);

            if (cancelResult.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(cancelResult) { StatusCode = StatusCodes.Status403Forbidden };

            if (cancelResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(cancelResult);

            if (cancelResult.StatusCode != HttpStatusCode.OK)
                return new ObjectResult(new Result { Message = cancelResult.Message }) { StatusCode = StatusCodes.Status500InternalServerError };

            return Ok(cancelResult);
        }


        /// <summary> Отмена заказа менеджером </summary>
        /// <param name="cancelOrderRequest"> Запрос на отмену заказа менеджером </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = "manager")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CancelOrderByManager(CancelOrderRequest cancelOrderRequest)
        {
            var cancelResult = await _ordersService.CancelOrderByManager(
                managerId: cancelOrderRequest.ManagerId,
                orderId: cancelOrderRequest.OrderId,
                confirmationString: cancelOrderRequest.ConfirmationString);

            if (cancelResult.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(cancelResult) { StatusCode = StatusCodes.Status403Forbidden };

            if (cancelResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(cancelResult);

            if (cancelResult.StatusCode != HttpStatusCode.OK)
                return new ObjectResult(new Result { Message = cancelResult.Message }) { StatusCode = StatusCodes.Status500InternalServerError };

            return Ok(cancelResult);
        }

        /// <summary> Подтверждение заказа покупателем </summary>
        /// <param name="confirmOrderRequest"> Запрос на подтверждение заказа покупателем </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = "buyer")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ConfirmOrderByByer(ConfirmOrderRequest confirmOrderRequest)
        {
            uint? buyerId = GetBuyerIdFromClaim();

            if (buyerId == null)
                return new ObjectResult(null) { StatusCode = StatusCodes.Status403Forbidden };

            var confirmResult = await _ordersService.ConfirmOrderByByer(
                buyerIdFromClaim: buyerId.Value,
                buyerIdFromRequest: confirmOrderRequest.BuyerId,
                orderId: confirmOrderRequest.OrderId,
                confirmationString: confirmOrderRequest.ConfirmationString);

            if (confirmResult.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(confirmResult) { StatusCode = StatusCodes.Status403Forbidden };

            if (confirmResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(confirmResult);

            if (confirmResult.StatusCode != HttpStatusCode.OK)
                return new ObjectResult(new Result { Message = confirmResult.Message }) { StatusCode = StatusCodes.Status500InternalServerError };

            return Ok(confirmResult);
        }


        /// <summary> Отметка заказа как доставленного покупателю </summary>
        /// <param name="markAsDeliveredRequest"> Запрос для отметки заказа как доставленного (покупателю) </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = "courier")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> MarkAsDeliveredToBuyer(MarkAsDeliveredRequest markAsDeliveredRequest) { throw new NotImplementedException(); }


        /// <summary> Отметка заказа как доставленного (в магазин) </summary>
        /// <param name="markAsDeliveredRequest"> Запрос для отметки заказа как доставленного (в магазин) </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = "manager")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> MarkAsDeliveredToShop(MarkAsDeliveredRequest markAsDeliveredRequest) { throw new NotImplementedException(); }


        /// <summary> Отметка заказа как полученного покупателем </summary>
        /// <param name="markAsReceivedRequest"> Запрос для отметки заказа как полученного покупателем </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = "manager, courier")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> MarkAsReceived(MarkAsReceivedRequest markAsReceivedRequest) { throw new NotImplementedException(); }
        
        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = "courier")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateCourierId(UpdateCourierIdRequest updateCourierIdRequest) { throw new NotImplementedException(); }


        /// <summary> Изменение адреса доставки заказа (со стороны покупателя) </summary>
        /// <param name="updateDeliveryAddressRequest"> Запрос на изменение адреса доставки заказа (со стороны покупателя) </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = "buyer")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateDeliveryAddressByBuyer(UpdateDeliveryAddressRequest updateDeliveryAddressRequest)
        {
            uint? buyerId = GetBuyerIdFromClaim();

            if (buyerId == null)
                return new ObjectResult(null) { StatusCode = StatusCodes.Status403Forbidden };

            var updateResult = await _ordersService.UpdateDeliveryAddressByBuyer(
                buyerIdFromClaim: buyerId.Value,
                buyerIdFromRequest: updateDeliveryAddressRequest.BuyerId,
                orderId: updateDeliveryAddressRequest.OrderId,
                deliveryAddress: updateDeliveryAddressRequest.DeliveryAddress);

            if (updateResult.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(updateResult) { StatusCode = StatusCodes.Status403Forbidden };

            if (updateResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(updateResult);

            if (updateResult.StatusCode != HttpStatusCode.OK)
                return new ObjectResult(new Result { Message = updateResult.Message }) { StatusCode = StatusCodes.Status500InternalServerError };

            return Ok(updateResult);
        }


        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = "manager, courier")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateDeliveryId(UpdateDeliveryIdRequest updateDeliveryIdRequest) { throw new NotImplementedException(); }


        /// <summary> Изменение дополнительной информации в заказе (со стороны покупателя) </summary>
        /// <param name="updateExtraInfoRequest"> Запрос на изменение дополнительной информации в заказе (со стороны покупателя) </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = "buyer")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateExtraInfoByBuyer(UpdateExtraInfoRequest updateExtraInfoRequest)
        {
            uint? buyerId = GetBuyerIdFromClaim();

            if (buyerId == null)
                return new ObjectResult(null) { StatusCode = StatusCodes.Status403Forbidden };

            var updateResult = await _ordersService.UpdateExtraInfoByBuyer(
                buyerIdFromClaim: buyerId.Value,
                buyerIdFromRequest: updateExtraInfoRequest.BuyerId,
                orderId: updateExtraInfoRequest.OrderId,
                extraInfo: updateExtraInfoRequest.ExtraInfo);

            if (updateResult.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(updateResult) { StatusCode = StatusCodes.Status403Forbidden };

            if (updateResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(updateResult);

            if (updateResult.StatusCode != HttpStatusCode.OK)
                return new ObjectResult(new Result { Message = updateResult.Message }) { StatusCode = StatusCodes.Status500InternalServerError };

            return Ok(updateResult);
        }

        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = "manager")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateManagerId(UpdateManagerIdRequest updateManagerIdRequest) { throw new NotImplementedException(); }

        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = "manager, courier")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateMassInGramsDimensions(UpdateMassInGramsDimensionsRequest updateMassInGramsDimensionsRequest) { throw new NotImplementedException(); }

        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = "manager")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdatePaymentInfoByManager(UpdatePaymentInfoByManagerRequest updatePaymentInfoByManagerRequest) { throw new NotImplementedException(); }


        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = "manager")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdatePlannedDeliveryTimeByManager(UpdatePlannedDeliveryTimeRequest updatePlannedDeliveryTimeRequest) { throw new NotImplementedException(); }


        /// <summary> Обновление (установка/смена/сброс) магазина выдачи заказа покупателем </summary>
        /// <param name="updateShopIdRequest"> Запрос на обновление (установку/смену/сброс) магазина выдачи заказа </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = "buyer")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateShopIdByBuyer(UpdateShopIdRequest updateShopIdRequest)
        {
            uint? buyerId = GetBuyerIdFromClaim();

            if (buyerId == null)
                return new ObjectResult(null) { StatusCode = StatusCodes.Status403Forbidden };

            var updateResult = await _ordersService.UpdateShopIdByBuyer(
                buyerIdFromClaim: buyerId.Value,
                buyerIdFromRequest: updateShopIdRequest.BuyerId,
                orderId: updateShopIdRequest.OrderId,
                shopId: updateShopIdRequest.ShopId);

            if (updateResult.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(updateResult) { StatusCode = StatusCodes.Status403Forbidden };
            
            if (updateResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(updateResult);

            if (updateResult.StatusCode != HttpStatusCode.OK)
                return new ObjectResult(new Result { Message = updateResult.Message }) { StatusCode = StatusCodes.Status500InternalServerError };

            return Ok(updateResult);
        }

        
        [NonAction]
        private uint? GetBuyerIdFromClaim()
        {
            var idFromClaimParsed = uint.TryParse(HttpContext.User.FindFirst(ClaimTypes.UserData)!.Value, out var idFromClaim);

            uint? buyerId = idFromClaimParsed ? idFromClaim : null;

            return buyerId;
        }
    }
}
