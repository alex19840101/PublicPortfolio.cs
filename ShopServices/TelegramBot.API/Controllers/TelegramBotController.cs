using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TelegramBot.API.Interfaces;

namespace TelegramBot.API.Controllers
{
    /// <summary> Контроллер для тестовой отправки уведомлений </summary>
    [ApiController]
    [Asp.Versioning.ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class TelegramBotController : ControllerBase
    {
        private readonly ILogger<TelegramBotController> _logger;
        private readonly ITelegramNotificationService _telegramNotificationService;

        /// <summary> Конструктор контроллера для тестовой отправки уведомлений </summary>
        public TelegramBotController(
            ITelegramNotificationService telegramNotificationService,
            ILogger<TelegramBotController> logger)
        {
            _telegramNotificationService = telegramNotificationService;
            _logger = logger;
        }

        /// <summary> Отправка сообщения </summary>
        /// <param name="telegramChatId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task SendMessage(
            [FromQuery] long telegramChatId,
            [FromBody] string message)
        {
            await _telegramNotificationService.SendMessage(
                chatId: new Telegram.Bot.Types.ChatId(telegramChatId),
                text: message);
        }
    }
}
