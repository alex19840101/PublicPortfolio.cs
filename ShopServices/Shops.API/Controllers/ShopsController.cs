using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shops.API.Contracts.Requests;
using ShopServices.Abstractions;
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
        public async Task<IActionResult> AddOrder(AddShopRequestDto addShopRequestDto)
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
    }
}
