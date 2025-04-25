using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Buyers.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BuyersController : ControllerBase
    {
        private readonly ILogger<BuyersController> _logger;

        public BuyersController(ILogger<BuyersController> logger)
        {
            _logger = logger;
        }
    }
}
