using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Warehouse.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WarehouseController : ControllerBase
    {
        private readonly ILogger<WarehouseController> _logger;

        public WarehouseController(ILogger<WarehouseController> logger)
        {
            _logger = logger;
        }

        //TODO: Warehouse.API
    }
}
