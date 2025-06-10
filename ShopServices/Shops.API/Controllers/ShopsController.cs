using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

        private readonly ILogger<ShopsController> _logger;

        /// <summary> Конструктор контроллера управления данными магазинов </summary>
        public ShopsController(ILogger<ShopsController> logger)
        {
            _logger = logger;
        }
        //TODO: Shops.API
    }
}
