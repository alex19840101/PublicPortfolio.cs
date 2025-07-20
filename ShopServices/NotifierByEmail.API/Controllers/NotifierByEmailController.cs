using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NotifierByEmail.API.Contracts;
using NotifierByEmail.API.Interfaces;
using ShopServices.Core.Auth;

namespace NotifierByEmail.API.Controllers
{
    /// <summary> Контроллер для тестовой отправки уведомлений </summary>
    [ApiController]
    [Asp.Versioning.ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize(Roles = Roles.Developer)]
    [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
    public class NotifierByEmailController : ControllerBase
    {
        private readonly ILogger<NotifierByEmailController> _logger;
        private readonly IEmailNotificationsService _emailNotificationsService;

        /// <summary> Конструктор контроллера для тестовой отправки уведомлений </summary>
        public NotifierByEmailController(
            IEmailNotificationsService emailNotificationsService,
            ILogger<NotifierByEmailController> logger)
        {
            _emailNotificationsService = emailNotificationsService;
            _logger = logger;
        }


        /// <summary> Отправка Email </summary>
        /// <param name="sendEmailRequest"> Запрос на отправку E-mail-письма </param>
        /// <returns></returns>
        [HttpPost]
        public async Task SendEmail(
            SendEmailRequest sendEmailRequest)
        {
            await _emailNotificationsService.SendEmail(
                emailSender: sendEmailRequest.EmailSender,
                emailReceiver: sendEmailRequest.EmailReceiver,
                topic: sendEmailRequest.Topic,
                emailBody: sendEmailRequest.EmailBody);
        }
    }
}
