using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
using ShopServices.Core.Auth;
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
        [Authorize(Roles = Roles.Buyer)]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> AddOrder(AddOrderRequest addOrderRequestDto)
        {
            uint? buyerId = GetUserIdFromClaim();

            if (buyerId == null)
                return new ObjectResult(null) { StatusCode = StatusCodes.Status403Forbidden };

            var userIdMismatch = ReturnResultAtBuyerMismatch(buyerId: addOrderRequestDto.BuyerId, userIdFromClaim: buyerId.Value);
            if (userIdMismatch != null)
                return new ObjectResult(userIdMismatch) { StatusCode = StatusCodes.Status403Forbidden };

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
        [Authorize(Roles = $"{Roles.Buyer}, {Roles.Manager}, {Roles.Courier}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> GetOrderInfoById(uint orderId)
        {
            uint? userId = GetUserIdFromClaim();
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)!.Value;
            if (!string.Equals(role, Roles.Buyer))
                userId = null;

            var order = await _ordersService.GetOrderInfoById(orderId, userId);

            if (order is null)
                return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

            return Ok(OrdersMapper.GetOrderResponseDto(order));
        }

        /// <summary> Получение информации о заказах покупателя для указанного временного интервала </summary>
        /// <param name="buyerId"> id покупателя </param>
        /// <param name="createdFromDt"> Создан от какого времени </param>
        /// <param name="createdToDt"> Создан до какого времени</param>
        /// <param name="byPage"> Количество товаров на странице </param>
        /// <param name="page"> Номер страницы </param>
        /// <returns> IEnumerable(OrderResponseDto) - перечень заказов покупателя для указанного временного интервала </returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderResponseDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [Authorize(Roles = Roles.Buyer)]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IEnumerable<OrderResponseDto>> GetOrdersByBuyerId(
            uint buyerId,
            DateTime createdFromDt,
            DateTime? createdToDt,
            [Range(1, 100)] uint byPage = 10,
            [Range(1, uint.MaxValue)] uint page = 1)
        {
            uint? buyerIdFromClaim = GetUserIdFromClaim();
            if (buyerIdFromClaim == null)
                return [];

            var buyerIdMismatch = ReturnResultAtBuyerMismatch(buyerId, userIdFromClaim: buyerIdFromClaim.Value);
            if (buyerIdMismatch != null)
                return [];

            var ordersCollection = await _ordersService.GetOrdersByBuyerId(
                buyerId: buyerId,
                buyerIdFromClaim: buyerIdFromClaim,
                createdFromDt: createdFromDt,
                createdToDt: createdToDt,
                byPage: byPage,
                page: page);

            if (!ordersCollection.Any())
                return [];

            return ordersCollection.GetOrderDtos();
        }

        /// <summary> Отмена заказа покупателем </summary>
        /// <param name="cancelOrderRequest"> Запрос на отмену заказа покупателем </param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = Roles.Buyer)]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> CancelOrderByBuyer(CancelOrderRequest cancelOrderRequest)
        {
            uint? buyerId = GetUserIdFromClaim();

            if (buyerId == null)
                return new ObjectResult(null) { StatusCode = StatusCodes.Status403Forbidden };

            var buyerIdMismatch = ReturnResultAtBuyerMismatch(buyerId: cancelOrderRequest.BuyerId, userIdFromClaim: buyerId.Value);
            if (buyerIdMismatch != null)
                return new ObjectResult(buyerIdMismatch) { StatusCode = StatusCodes.Status403Forbidden };

            var cancelResult = await _ordersService.CancelOrderByBuyer(
                buyerIdFromClaim: buyerId.Value,
                buyerIdFromRequest: cancelOrderRequest.BuyerId,
                orderId: cancelOrderRequest.OrderId,
                confirmationString: cancelOrderRequest.ConfirmationString,
                comment: cancelOrderRequest.Comment);

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
        [HttpDelete]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = Roles.Manager)]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> CancelOrderByManager(CancelOrderRequest cancelOrderRequest)
        {
            uint? managerId = GetUserIdFromClaim();

            if (managerId == null)
                return new ObjectResult(null) { StatusCode = StatusCodes.Status403Forbidden };

            var managerIdMismatch = ReturnResultAtManagerMismatch(managerId: cancelOrderRequest.ManagerId, userIdFromClaim: managerId.Value);
            if (managerIdMismatch != null)
                return new ObjectResult(managerIdMismatch) { StatusCode = StatusCodes.Status403Forbidden };

            var cancelResult = await _ordersService.CancelOrderByManager(
                managerId: cancelOrderRequest.ManagerId,
                orderId: cancelOrderRequest.OrderId,
                confirmationString: cancelOrderRequest.ConfirmationString,
                comment: cancelOrderRequest.Comment);

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
        [Authorize(Roles = Roles.Buyer)]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> ConfirmOrderByBuyer(ConfirmOrderRequest confirmOrderRequest)
        {
            uint? buyerId = GetUserIdFromClaim();

            if (buyerId == null)
                return new ObjectResult(null) { StatusCode = StatusCodes.Status403Forbidden };

            var buyerIdMismatch = ReturnResultAtBuyerMismatch(buyerId: confirmOrderRequest.BuyerId, userIdFromClaim: buyerId.Value);
            if (buyerIdMismatch != null)
                return new ObjectResult(buyerIdMismatch) { StatusCode = StatusCodes.Status403Forbidden };

            var confirmResult = await _ordersService.ConfirmOrderByBuyer(
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


        /// <summary> Отметка заказа как доставленного в магазин/покупателю </summary>
        /// <param name="markAsDeliveredRequest"> Запрос для отметки заказа как доставленного в магазин/покупателю </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = Roles.Courier)]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> MarkAsDeliveredToBuyer(MarkAsDeliveredRequest markAsDeliveredRequest)
        {
            uint? courierId = GetUserIdFromClaim();

            if (courierId == null)
                return new ObjectResult(null) { StatusCode = StatusCodes.Status403Forbidden };

            var courierIdMismatch = ReturnResultAtCourierMismatch(courierId: markAsDeliveredRequest.CourierId, userIdFromClaim: courierId.Value);
            if (courierIdMismatch != null)
                return new ObjectResult(courierIdMismatch) { StatusCode = StatusCodes.Status403Forbidden };

            var result = await _ordersService.MarkAsDeliveredToBuyer(
                    orderId: markAsDeliveredRequest.OrderId,
                    comment: markAsDeliveredRequest.Comment,
                    courierId: markAsDeliveredRequest.CourierId);

            if (result.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(result) { StatusCode = StatusCodes.Status403Forbidden };

            if (result.StatusCode == HttpStatusCode.NotFound)
                return NotFound(result);

            if (result.StatusCode != HttpStatusCode.OK)
                return new ObjectResult(new Result { Message = result.Message }) { StatusCode = StatusCodes.Status500InternalServerError };

            return Ok(result);
        }


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
        [Authorize(Roles = Roles.Manager)]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> MarkAsDeliveredToShop(MarkAsDeliveredRequest markAsDeliveredRequest)
        {
            uint? managerId = GetUserIdFromClaim();

            if (managerId == null)
                return new ObjectResult(null) { StatusCode = StatusCodes.Status403Forbidden };

            var managerIdMismatch = ReturnResultAtManagerMismatch(managerId: markAsDeliveredRequest.ManagerId, userIdFromClaim: managerId.Value);
            if (managerIdMismatch != null)
                return new ObjectResult(managerIdMismatch) { StatusCode = StatusCodes.Status403Forbidden };

            var result = await _ordersService.MarkAsDeliveredToShop(
                    orderId: markAsDeliveredRequest.OrderId,
                    comment: markAsDeliveredRequest.Comment,
                    managerId: markAsDeliveredRequest.ManagerId);

            if (result.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(result) { StatusCode = StatusCodes.Status403Forbidden };

            if (result.StatusCode == HttpStatusCode.NotFound)
                return NotFound(result);

            if (result.StatusCode != HttpStatusCode.OK)
                return new ObjectResult(new Result { Message = result.Message }) { StatusCode = StatusCodes.Status500InternalServerError };

            return Ok(result);
        }


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
        [Authorize(Roles = $"{Roles.Manager}, {Roles.Courier}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> MarkAsReceived(MarkAsReceivedRequest markAsReceivedRequest)
        {
            uint? userId = GetUserIdFromClaim();

            if (userId == null)
                return new ObjectResult(null) { StatusCode = StatusCodes.Status403Forbidden };

            var userIdMismatch = ReturnResultAtManagerMismatch(managerId: markAsReceivedRequest.ManagerId, userIdFromClaim: userId.Value) ??
                                       ReturnResultAtCourierMismatch(courierId: markAsReceivedRequest.CourierId, userIdFromClaim: userId.Value);
            if (userIdMismatch != null)
                return new ObjectResult(userIdMismatch) { StatusCode = StatusCodes.Status403Forbidden };


            var result = await _ordersService.MarkAsReceived(
                    orderId: markAsReceivedRequest.OrderId,
                    comment: markAsReceivedRequest.Comment,
                    managerId: markAsReceivedRequest.ManagerId,
                    courierId: markAsReceivedRequest.CourierId);

            if (result.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(result) { StatusCode = StatusCodes.Status403Forbidden };

            if (result.StatusCode == HttpStatusCode.NotFound)
                return NotFound(result);

            if (result.StatusCode != HttpStatusCode.OK)
                return new ObjectResult(new Result { Message = result.Message }) { StatusCode = StatusCodes.Status500InternalServerError };

            return Ok(result);
        }

        /// <summary> Смена/сброс курьера, доставляющего заказ </summary>
        /// <param name="updateCourierIdRequest"> Запрос для смены/сброса курьера, доставляющего заказ </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = Roles.Courier)]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> UpdateCourierId(UpdateCourierIdRequest updateCourierIdRequest)
        {
            uint? courierId = GetUserIdFromClaim();

            if (courierId == null)
                return new ObjectResult(null) { StatusCode = StatusCodes.Status403Forbidden };

            var courierIdMismatch = ReturnResultAtCourierMismatch(courierId: updateCourierIdRequest.CourierId, userIdFromClaim: courierId.Value);
            if (courierIdMismatch != null)
                return new ObjectResult(courierIdMismatch) { StatusCode = StatusCodes.Status403Forbidden };


            var result = await _ordersService.UpdateCourierId(
                    orderId: updateCourierIdRequest.OrderId,
                    comment: updateCourierIdRequest.Comment,
                    courierId: updateCourierIdRequest.CourierId);

            if (result.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(result) { StatusCode = StatusCodes.Status403Forbidden };

            if (result.StatusCode == HttpStatusCode.NotFound)
                return NotFound(result);

            if (result.StatusCode != HttpStatusCode.OK)
                return new ObjectResult(new Result { Message = result.Message }) { StatusCode = StatusCodes.Status500InternalServerError };

            return Ok(result);
        }


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
        [Authorize(Roles = Roles.Buyer)]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> UpdateDeliveryAddressByBuyer(UpdateDeliveryAddressRequest updateDeliveryAddressRequest)
        {
            uint? buyerId = GetUserIdFromClaim();

            if (buyerId == null)
                return new ObjectResult(null) { StatusCode = StatusCodes.Status403Forbidden };

            var buyerIdMismatch = ReturnResultAtBuyerMismatch(buyerId: updateDeliveryAddressRequest.BuyerId, userIdFromClaim: buyerId.Value);
            if (buyerIdMismatch != null)
                return new ObjectResult(buyerIdMismatch) { StatusCode = StatusCodes.Status403Forbidden };

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


        /// <summary>
        /// Смена идентификатора доставки (менеджером/курьером) (в случаях изменений адреса/магазина доставки/отмены заказа...)
        /// </summary>
        /// <param name="updateDeliveryIdRequest"> Запрос для смены идентификатора доставки (менеджером/курьером) (в случаях изменений адреса/магазина доставки/отмены заказа...) </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = $"{Roles.Manager}, {Roles.Courier}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> UpdateDeliveryId(UpdateDeliveryIdRequest updateDeliveryIdRequest)
        {
            uint? userId = GetUserIdFromClaim();

            if (userId == null)
                return new ObjectResult(null) { StatusCode = StatusCodes.Status403Forbidden };

            var userIdMismatch = ReturnResultAtManagerMismatch(managerId: updateDeliveryIdRequest.ManagerId, userIdFromClaim: userId.Value) ??
                                        ReturnResultAtCourierMismatch(courierId: updateDeliveryIdRequest.CourierId, userIdFromClaim: userId.Value);
            if (userIdMismatch != null)
                return new ObjectResult(userIdMismatch) { StatusCode = StatusCodes.Status403Forbidden };

            var updateResult = await _ordersService.UpdateDeliveryId(
                orderId: updateDeliveryIdRequest.OrderId,
                deliveryId: updateDeliveryIdRequest.DeliveryId,
                managerId: updateDeliveryIdRequest.ManagerId,
                courierId: updateDeliveryIdRequest.CourierId,
                comment : updateDeliveryIdRequest.Comment);

            if (updateResult.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(updateResult) { StatusCode = StatusCodes.Status403Forbidden };

            if (updateResult.StatusCode == HttpStatusCode.NotFound)
                return NotFound(updateResult);

            if (updateResult.StatusCode != HttpStatusCode.OK)
                return new ObjectResult(new Result { Message = updateResult.Message }) { StatusCode = StatusCodes.Status500InternalServerError };

            return Ok(updateResult);
        }


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
        [Authorize(Roles = Roles.Buyer)]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> UpdateExtraInfoByBuyer(UpdateExtraInfoRequest updateExtraInfoRequest)
        {
            uint? buyerId = GetUserIdFromClaim();

            if (buyerId == null)
                return new ObjectResult(null) { StatusCode = StatusCodes.Status403Forbidden };

            var buyerIdMismatch = ReturnResultAtBuyerMismatch(buyerId: updateExtraInfoRequest.BuyerId, userIdFromClaim: buyerId.Value);
            if (buyerIdMismatch != null)
                return new ObjectResult(buyerIdMismatch) { StatusCode = StatusCodes.Status403Forbidden };

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


        /// <summary> Смена менеджера, обслуживающего заказ </summary>
        /// <param name="updateManagerIdRequest"> Запрос для смены менеджера, обслуживающего заказ </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = Roles.Manager)]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> UpdateManagerId(UpdateManagerIdRequest updateManagerIdRequest)
        {
            uint? managerId = GetUserIdFromClaim();

            if (managerId == null)
                return new ObjectResult(null) { StatusCode = StatusCodes.Status403Forbidden };

            var managerIdMismatch = ReturnResultAtManagerMismatch(managerId: updateManagerIdRequest.ManagerId, userIdFromClaim: managerId.Value);
            if (managerIdMismatch != null)
                return new ObjectResult(managerIdMismatch) { StatusCode = StatusCodes.Status403Forbidden };

            var result = await _ordersService.UpdateManagerId(
                    orderId: updateManagerIdRequest.OrderId,
                    comment: updateManagerIdRequest.Comment,
                    managerId: updateManagerIdRequest.ManagerId);

            if (result.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(result) { StatusCode = StatusCodes.Status403Forbidden };

            if (result.StatusCode == HttpStatusCode.NotFound)
                return NotFound(result);

            if (result.StatusCode != HttpStatusCode.OK)
                return new ObjectResult(new Result { Message = result.Message }) { StatusCode = StatusCodes.Status500InternalServerError };

            return Ok(result);
        }


        /// <summary> Уточнение массы/габаритов заказа (менеджером/курьером) </summary>
        /// <param name="updateMassInGramsDimensionsRequest"> Запрос для уточнения массы/габаритов заказа (менеджером/курьером) </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = $"{Roles.Manager}, {Roles.Courier}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> UpdateMassInGramsDimensions(UpdateMassInGramsDimensionsRequest updateMassInGramsDimensionsRequest)
        {
            uint? userId = GetUserIdFromClaim();

            if (userId == null)
                return new ObjectResult(null) { StatusCode = StatusCodes.Status403Forbidden };

            var userIdMismatch = ReturnResultAtManagerMismatch(managerId: updateMassInGramsDimensionsRequest.ManagerId, userIdFromClaim: userId.Value) ??
                                       ReturnResultAtCourierMismatch(courierId: updateMassInGramsDimensionsRequest.CourierId, userIdFromClaim: userId.Value);
            if (userIdMismatch != null)
                return new ObjectResult(userIdMismatch) { StatusCode = StatusCodes.Status403Forbidden };

            var result = await _ordersService.UpdateMassInGramsDimensions(
                    orderId: updateMassInGramsDimensionsRequest.OrderId,
                    massInGrams: updateMassInGramsDimensionsRequest.MassInGrams,
                    dimensions: updateMassInGramsDimensionsRequest.Dimensions,
                    comment: updateMassInGramsDimensionsRequest.Comment,
                    managerId: updateMassInGramsDimensionsRequest.ManagerId,
                    courierId: updateMassInGramsDimensionsRequest.CourierId,
                    deliveryId: updateMassInGramsDimensionsRequest.DeliveryId);

            if (result.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(result) { StatusCode = StatusCodes.Status403Forbidden };

            if (result.StatusCode == HttpStatusCode.NotFound)
                return NotFound(result);

            if (result.StatusCode != HttpStatusCode.OK)
                return new ObjectResult(new Result { Message = result.Message }) { StatusCode = StatusCodes.Status500InternalServerError };

            return Ok(result);
        }


        /// <summary> Обновление информации об оплате (со стороны менеджера/курьера, обслуживающего заказ) </summary>
        /// <param name="updatePaymentInfoByManagerRequest"> Запрос для обновления информации об оплате (со стороны менеджера/курьера, обслуживающего заказ) </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = $"{Roles.Manager}, {Roles.Courier}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> UpdatePaymentInfo(UpdatePaymentInfoByManagerRequest updatePaymentInfoByManagerRequest)
        {
            uint? userId = GetUserIdFromClaim();

            if (userId == null)
                return new ObjectResult(null) { StatusCode = StatusCodes.Status403Forbidden };

            var userIdMismatch = ReturnResultAtManagerMismatch(managerId: updatePaymentInfoByManagerRequest.ManagerId, userIdFromClaim: userId.Value) ??
                                       ReturnResultAtCourierMismatch(courierId: updatePaymentInfoByManagerRequest.CourierId, userIdFromClaim: userId.Value);
            if (userIdMismatch != null)
                return new ObjectResult(userIdMismatch) { StatusCode = StatusCodes.Status403Forbidden };

            var result = await _ordersService.UpdatePaymentInfo(
                    orderId: updatePaymentInfoByManagerRequest.OrderId,
                    paymentInfo: updatePaymentInfoByManagerRequest.PaymentInfo,
                    comment: updatePaymentInfoByManagerRequest.Comment,
                    managerId: updatePaymentInfoByManagerRequest.ManagerId,
                    courierId: updatePaymentInfoByManagerRequest.CourierId);

            if (result.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(result) { StatusCode = StatusCodes.Status403Forbidden };

            if (result.StatusCode == HttpStatusCode.NotFound)
                return NotFound(result);

            if (result.StatusCode != HttpStatusCode.OK)
                return new ObjectResult(new Result { Message = result.Message }) { StatusCode = StatusCodes.Status500InternalServerError };

            return Ok(result);
        }


        /// <summary> Обновление планируемого срока поставки заказа менеджером, обслуживающим заказ </summary>
        /// <param name="updatePlannedDeliveryTimeRequest"> Запрос для обновления планируемого срока поставки заказа менеджером, обслуживающим заказ</param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = Roles.Manager)]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> UpdatePlannedDeliveryTimeByManager(UpdatePlannedDeliveryTimeRequest updatePlannedDeliveryTimeRequest)
        {
            uint? managerId = GetUserIdFromClaim();

            if (managerId == null)
                return new ObjectResult(null) { StatusCode = StatusCodes.Status403Forbidden };

            var managerIdMismatch = ReturnResultAtManagerMismatch(managerId: updatePlannedDeliveryTimeRequest.ManagerId, userIdFromClaim: managerId.Value);
            if (managerIdMismatch != null)
                return new ObjectResult(managerIdMismatch) { StatusCode = StatusCodes.Status403Forbidden };

            var result = await _ordersService.UpdatePlannedDeliveryTimeByManager(
                    orderId: updatePlannedDeliveryTimeRequest.OrderId,
                    plannedDeliveryTime: updatePlannedDeliveryTimeRequest.PlannedDeliveryTime,
                    comment: updatePlannedDeliveryTimeRequest.Comment,
                    managerId: updatePlannedDeliveryTimeRequest.ManagerId);

            if (result.StatusCode == HttpStatusCode.Forbidden)
                return new ObjectResult(result) { StatusCode = StatusCodes.Status403Forbidden };

            if (result.StatusCode == HttpStatusCode.NotFound)
                return NotFound(result);

            if (result.StatusCode != HttpStatusCode.OK)
                return new ObjectResult(new Result { Message = result.Message }) { StatusCode = StatusCodes.Status500InternalServerError };

            return Ok(result);
        }


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
        [Authorize(Roles = Roles.Buyer)]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> UpdateShopIdByBuyer(UpdateShopIdRequest updateShopIdRequest)
        {
            uint? buyerId = GetUserIdFromClaim();

            if (buyerId == null)
                return new ObjectResult(null) { StatusCode = StatusCodes.Status403Forbidden };

            var buyerIdMismatch = ReturnResultAtBuyerMismatch(buyerId: updateShopIdRequest.BuyerId, userIdFromClaim: buyerId.Value);
            if (buyerIdMismatch != null)
                return new ObjectResult(buyerIdMismatch) { StatusCode = StatusCodes.Status403Forbidden };

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
        private uint? GetUserIdFromClaim()
        {
            var idFromClaimParsed = uint.TryParse(HttpContext.User.FindFirst(ClaimTypes.UserData)!.Value, out var idFromClaim);

            uint? userId = idFromClaimParsed ? idFromClaim : null;

            return userId;
        }

        [NonAction]
        private static Result? ReturnResultAtBuyerMismatch(uint? buyerId, uint userIdFromClaim)
        {
            if (buyerId is null)
                return null;

            if (buyerId != userIdFromClaim)
                return new Result
                {
                    Message = $"{ResultMessager.BUYER_ID_MISMATCH}: from Claim.UserData:{userIdFromClaim}, from request:{buyerId}",
                    StatusCode = HttpStatusCode.Forbidden
                };

            return null;
        }


        [NonAction]
        private static Result? ReturnResultAtCourierMismatch(uint? courierId, uint userIdFromClaim)
        {
            if (courierId is null)
                return null;

            if (courierId != userIdFromClaim)
                return new Result
                {
                    Message = $"{ResultMessager.COURIER_ID_MISMATCH}: from Claim.UserData:{userIdFromClaim}, from request:{courierId}",
                    StatusCode = HttpStatusCode.Forbidden
                };

            return null;
        }

        [NonAction]
        private static Result? ReturnResultAtManagerMismatch(uint? managerId, uint userIdFromClaim)
        {
            if (managerId is null)
                return null;

            if (managerId != userIdFromClaim)
                return new Result
                {
                    Message = $"{ResultMessager.MANAGER_ID_MISMATCH}: from Claim.UserData:{userIdFromClaim}, from request:{managerId}",
                    StatusCode = HttpStatusCode.Forbidden
                };

            return null;
        }
    }
}
