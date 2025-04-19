using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Shops.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShopsController : ControllerBase
    {

        private readonly ILogger<ShopsController> _logger;

        public ShopsController(ILogger<ShopsController> logger)
        {
            _logger = logger;
        }
    }
}
