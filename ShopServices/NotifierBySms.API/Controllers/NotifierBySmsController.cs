using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NotifierBySms.API.Contracts;
using NotifierBySms.API.Interfaces;
using ShopServices.Core.Auth;

namespace NotifierBySms.API.Controllers
{
    /// <summary> Контроллер для тестовой отправки уведомлений </summary>
    [ApiController]
    [Asp.Versioning.ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize(Roles = Roles.Developer)]
    [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
    public class NotifierBySmsController : ControllerBase
    {
        private readonly ISmsNotificationsService _smsNotificationsService;
        private readonly ILogger<NotifierBySmsController> _logger;

        /// <summary> Конструктор контроллера для тестовой отправки уведомлений </summary>
        public NotifierBySmsController(
            ISmsNotificationsService _smsNotificationsService,
            ILogger<NotifierBySmsController> logger)
        {
            _logger = logger;
        }

        /// <summary> Отправка SMS-уведомления </summary>
        /// <param name="sendSmsRequest"> Запрос на отправку SMS-уведомления </param>
        /// <returns></returns>
        [HttpPost]
        public async Task SendSms(
            SendSmsRequest sendSmsRequest)
        {
            await _smsNotificationsService.SendSmsNotification(
                phoneSender: sendSmsRequest.PhoneSender,
                phoneReceiver: sendSmsRequest.PhoneReceiver,
                message: sendSmsRequest.Message);
        }
    }
}
