using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Managers.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManagersController : ControllerBase
    {
        private readonly ILogger<ManagersController> _logger;

        public ManagersController(ILogger<ManagersController> logger)
        {
            _logger = logger;
        }
    }
}
