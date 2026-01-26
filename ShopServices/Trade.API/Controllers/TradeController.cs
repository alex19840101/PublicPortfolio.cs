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

namespace Trade.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TradeController : ControllerBase
    {
        private readonly ITradeService _tradeService;
        private readonly ILogger<TradeController> _logger;

        public TradeController(ILogger<TradeController> logger)
        {
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

            var createResult = await _tradeService.AddTrade(TradeMapper.PrepareCoreTrade(addPaymentRequestDto));

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
        [Authorize(Roles = $"{Roles.Buyer}, {Roles.Manager}, {Roles.Courier}")]
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

            var createResult = await _tradeService.AddRefund(TradeMapper.PrepareCoreTrade(addRefundRequestDto));

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
