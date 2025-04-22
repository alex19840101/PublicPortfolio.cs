using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Couriers.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CouriersController : ControllerBase
    {
        private readonly ILogger<CouriersController> _logger;

        public CouriersController(ILogger<CouriersController> logger)
        {
            _logger = logger;
        }
    }
}
