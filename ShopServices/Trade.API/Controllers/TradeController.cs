using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Trade.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TradeController : ControllerBase
    {
        private readonly ILogger<TradeController> _logger;

        public TradeController(ILogger<TradeController> logger)
        {
            _logger = logger;
        }
    }
}
