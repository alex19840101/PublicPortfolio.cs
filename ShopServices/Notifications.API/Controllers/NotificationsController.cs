using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Notifications.API.Contracts.Requests;
using Notifications.API.Contracts.Responses;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Auth;
using ShopServices.Core.Services;

namespace Notifications.API.Controllers
{
    /// <summary> Контроллер управления уведомлениями (для ручных проверок) </summary>
    [ApiController]
    [Asp.Versioning.ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize(Roles = $"{Roles.Admin}, {Roles.Manager}")]
    [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationsService _notificationsService;
        private readonly ILogger<NotificationsController> _logger;

        /// <summary> Конструктор контроллера управления уведомлениями </summary>
        public NotificationsController(
            INotificationsService notificationService,
            ILogger<NotificationsController> logger)
        {
            _notificationsService = notificationService;
            _logger = logger;
        }

        /// <summary> Добавление однократного уведомления </summary>
        /// <param name="addNotificationRequestDto"> Запрос на добавление однократного уведомления </param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> AddNotification(AddNotificationRequestDto addNotificationRequestDto)
        {
            uint? employeeId = GetUserIdFromClaim();

            if (employeeId == null)
                return new ObjectResult(null) { StatusCode = StatusCodes.Status403Forbidden };

            var createResult = await _notificationsService.AddNotification(
                notification: NotificationsMapper.PrepareCoreNotification(addNotificationRequestDto, employeeId.Value));

            if (createResult.StatusCode == HttpStatusCode.BadRequest)
                return new BadRequestObjectResult(new ProblemDetails { Title = createResult.Message });

            if (createResult.StatusCode == HttpStatusCode.Conflict)
                return new ConflictObjectResult(new Result
                {
                    Message = createResult.Message,
                    StatusCode = createResult.StatusCode
                });

            if (createResult.StatusCode != HttpStatusCode.Created)
                return new ObjectResult(new Result { Message = createResult.Message }) { StatusCode = StatusCodes.Status500InternalServerError };

            var result = new Result
            {
                Id = createResult!.Id!.Value,
                Message = createResult.Message
            };
            _logger.LogInformation((EventId)(int)result!.Id!, @"added Notification {result.Id}", result!.Id!);

            return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
        }

        /// <summary> Получение информации об однократном Email-уведомлении по id </summary>
        /// <param name="notificationId"> id Email-уведомления </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(NotificationDataResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetEmailNotificationDataById(ulong notificationId)
        {
            var notification = await _notificationsService.GetEmailNotificationDataById(notificationId);

            if (notification is null)
                return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

            return Ok(NotificationsMapper.GetNotificationDataResponseDto(notification));
        }

        /// <summary> Получение информации об однократном телефонном уведомлении по id </summary>
        /// <param name="notificationId"> id телефонного уведомления </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(NotificationDataResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPhoneNotificationDataById(ulong notificationId)
        {
            var notification = await _notificationsService.GetPhoneNotificationDataById(notificationId);

            if (notification is null)
                return NotFound(new Result { Message = ResultMessager.NOT_FOUND });

            return Ok(NotificationsMapper.GetNotificationDataResponseDto(notification));
        }

        /// <summary> Получение информации об уведомлениях покупателя </summary>
        /// <param name="buyerId"> *Id покупателя </param>
        /// <param name="orderId"> Id заказа </param>
        /// <param name="byPage"> Количество на странице </param>
        /// <param name="page"> Номер страницы </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<NotificationDataResponseDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.Forbidden)]
        public async Task<IEnumerable<NotificationDataResponseDto>> GetNotifications(
            uint buyerId,
            uint? orderId = null,
            [Range(1, 100)] uint byPage = 10,
            [Range(1, uint.MaxValue)] uint page = 1)
        {
            var notificationsCollection = await _notificationsService.GetNotifications(
                buyerId: buyerId,
                orderId: orderId,
                byPage: byPage,
                page: page);

            if (!notificationsCollection.Any())
                return [];

            return notificationsCollection.GetNotificationsDtos();
        }

        [NonAction]
        private uint? GetUserIdFromClaim()
        {
            var idFromClaimParsed = uint.TryParse(HttpContext.User.FindFirst(ClaimTypes.UserData)!.Value, out var idFromClaim);

            uint? userId = idFromClaimParsed ? idFromClaim : null;

            return userId;
        }
    }
}
