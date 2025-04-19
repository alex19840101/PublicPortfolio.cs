using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Goods.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GoodsController : ControllerBase
    {
        private readonly ILogger<GoodsController> _logger;

        public GoodsController(ILogger<GoodsController> logger)
        {
            _logger = logger;
        }
    }
}
