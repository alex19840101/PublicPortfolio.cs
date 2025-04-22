using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GoodsGroups.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GoodsGroupsController : ControllerBase
    {
        private readonly ILogger<GoodsGroupsController> _logger;

        public GoodsGroupsController(ILogger<GoodsGroupsController> logger)
        {
            _logger = logger;
        }
    }
}
