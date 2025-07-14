using System;
using Microsoft.AspNetCore.Mvc;
using ShopServices.Core.Services;

namespace NotificationsSender.Controllers;

/// <summary> Контроллер управления рассыльщиком уведомлений </summary>
[ApiController]
[Asp.Versioning.ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[Produces("application/json")]
[Consumes("application/json")]
public class NotificationsSenderController : ControllerBase
{
    private readonly INotificationsSenderService _notificationsSenderService;

    /// <summary> Конструктор контроллера управления рассыльщиком уведомлений </summary>
    public NotificationsSenderController(INotificationsSenderService notificationsSenderService)
    {
        _notificationsSenderService = notificationsSenderService;
    }

    [HttpGet]
    public string GetStatus() => DateTime.Now.ToString();

}
