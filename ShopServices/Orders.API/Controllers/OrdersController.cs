using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orders.API.Contracts.Requests;
using ShopServices.Abstractions;
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

            var result = new Result
            {
                Id = createResult!.Id!.Value,
                Message = createResult.Message
            };
            _logger.LogInformation((EventId)(int)result!.Id!, @"added Order {result.Id}", result!.Id!);

            return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
        }
    }
}
