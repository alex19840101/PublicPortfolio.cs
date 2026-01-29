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
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Auth;
using ShopServices.Core.Services;
using Trade.API.Contracts.Requests;
using Trade.API.Contracts.Responses;

namespace Trade.API.Controllers
{
    /// <summary> Контроллер управления транзакциями оплат/возвратов </summary>
    [ApiController]
    [Asp.Versioning.ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class TradeController : ControllerBase
    {
        private readonly ITradeService _tradeService;
        private readonly ILogger<TradeController> _logger;

        public TradeController(
            ITradeService tradeService,
            ILogger<TradeController> logger)
        {
            _tradeService = tradeService;
            _logger = logger;
        }
        //TODO: Trade.API

        /// <summary> Добавление оплаты </summary>
        /// <param name="addPaymentRequestDto"> Запрос на добавление оплаты </param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = $"{Roles.Buyer}, {Roles.Manager}, {Roles.Courier}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> AddPayment(AddPaymentRequest addPaymentRequestDto)
        {
            uint? buyerId = GetUserIdFromClaim();

            if (buyerId != null && addPaymentRequestDto.BuyerId != null)
            {
                var userIdMismatch = ReturnResultAtBuyerMismatch(buyerId: addPaymentRequestDto.BuyerId, userIdFromClaim: buyerId.Value);
                if (userIdMismatch != null)
                    return new ObjectResult(userIdMismatch) { StatusCode = StatusCodes.Status403Forbidden };
            }

            var createResult = await _tradeService.AddPayment(TradeMapper.PrepareCorePayment(addPaymentRequestDto));

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
            _logger.LogInformation((EventId)(int)result!.Id!, @"added Payment {result.Id}", result!.Id!);

            return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
        }

        /// <summary> Добавление возврата </summary>
        /// <param name="addRefundRequestDto"> Запрос на добавление возврата </param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = $"{Roles.Manager}, {Roles.Courier}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> AddRefund(AddRefundRequest addRefundRequestDto)
        {
            uint? buyerId = GetUserIdFromClaim();

            if (buyerId != null && addRefundRequestDto.BuyerId != null)
            {
                var userIdMismatch = ReturnResultAtBuyerMismatch(buyerId: addRefundRequestDto.BuyerId, userIdFromClaim: buyerId.Value);
                if (userIdMismatch != null)
                    return new ObjectResult(userIdMismatch) { StatusCode = StatusCodes.Status403Forbidden };
            }

            var createResult = await _tradeService.AddRefund(TradeMapper.PrepareCoreRefund(addRefundRequestDto));

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
            _logger.LogInformation((EventId)(int)result!.Id!, @"added Refund {result.Id}", result!.Id!);

            return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
        }

        /// <summary> Получение информации о транзакции оплаты/возврата по её id </summary>
        /// <param name="transactionId"> id транзакции оплаты/возврата </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(TransactionInfoResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [Authorize(Roles = $"{Roles.Buyer}, {Roles.Manager}, {Roles.Courier}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IActionResult> GetTransactionInfoById(long transactionId)
        {
            uint? userId = GetUserIdFromClaim();
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)!.Value;
            if (!string.Equals(role, Roles.Buyer))
                userId = null;

            var order = await _tradeService.GetTransactionInfoById(transactionId, userId);

            if (order is null)
                return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

            return Ok(TradeMapper.GetTransactionInfoResponseDto(order));
        }

        /// <summary> Получение информации о транзакциях оплаты/возврата покупателя для указанного временного интервала </summary>
        /// <param name="buyerId"> id покупателя </param>
        /// <param name="createdFromDt"> Создан от какого времени </param>
        /// <param name="createdToDt"> Создан до какого времени</param>
        /// <param name="byPage"> Количество товаров на странице </param>
        /// <param name="page"> Номер страницы </param>
        /// <returns> IEnumerable(TransactionInfoResponseDto) - перечень транзакциях оплаты/возврата покупателя для указанного временного интервала </returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TransactionInfoResponseDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [Authorize(Roles = $"{Roles.Buyer}, {Roles.Manager}, {Roles.Courier}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IEnumerable<TransactionInfoResponseDto>> GetTransactionInfosByBuyerId(
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

            var trxCollection = await _tradeService.GetTransactionInfosByBuyerId(
                buyerId: buyerId,
                buyerIdFromClaim: buyerIdFromClaim,
                createdFromDt: createdFromDt,
                createdToDt: createdToDt,
                byPage: byPage,
                page: page);

            if (!trxCollection.Any())
                return [];

            return trxCollection.GetTrxDtos();
        }

        /// <summary> Получение информации о транзакциях оплаты/возврата по id заказа </summary>
        /// <param name="orderId"> id заказа </param>
        /// <param name="buyerId"> id покупателя </param>
        /// <returns> IEnumerable(TransactionInfoResponseDto) - перечень транзакций оплаты/возврата по id заказа </returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TransactionInfoResponseDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [Authorize(Roles = $"{Roles.Buyer}, {Roles.Manager}, {Roles.Courier}")]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public async Task<IEnumerable<TransactionInfoResponseDto>> GetTransactionInfosByOrderId(
            uint orderId,
            uint buyerId)
        {
            uint? buyerIdFromClaim = GetUserIdFromClaim();
            if (buyerIdFromClaim == null)
                return [];

            var buyerIdMismatch = ReturnResultAtBuyerMismatch(buyerId, userIdFromClaim: buyerIdFromClaim.Value);
            if (buyerIdMismatch != null)
                return [];

            var trxCollection = await _tradeService.GetTransactionInfosByOrderId(
                orderId,
                buyerId: buyerId,
                buyerIdFromClaim: buyerIdFromClaim);

            if (!trxCollection.Any())
                return [];

            return trxCollection.GetTrxDtos();
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
    }
}
