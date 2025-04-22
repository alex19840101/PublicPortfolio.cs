using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopServices.Core.Services;

namespace Goods.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GoodsController : ControllerBase
    {
        private readonly IGoodsService goodsService;
        private readonly ILogger<GoodsController> _logger;

        public GoodsController(
            IGoodsService _goodsService,
            ILogger<GoodsController> logger)
        {
            _goodsService = goodsService;
            _logger = logger;
        }
    }
}
