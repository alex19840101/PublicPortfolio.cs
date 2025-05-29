using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Notifications.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(ILogger<NotificationsController> logger)
        {
            _logger = logger;
        }
    }
}
