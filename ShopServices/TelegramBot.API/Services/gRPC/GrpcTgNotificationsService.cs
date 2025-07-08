using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TelegramBot.API.Interfaces;
using TelegramBot.API.Services.gRPC.Notifications;
using System.Net;

namespace TelegramBot.API.Services.gRPC
{
    /// <summary> Внутренний gRPC-сервис для работы с Telegram-уведомлениями </summary>
    public class GrpcTgNotificationsService : TelegramBot.API.Services.gRPC.Notifications.GrpcTgNotifications.GrpcTgNotificationsBase
    {
        private readonly ITelegramNotificationService _telegramNotificationService;
        private readonly ILogger<GrpcTgNotificationsService> _logger;

        /// <summary> Констуктор внутреннего gRPC-сервиса для работы с Telegram-уведомлениями </summary>
        public GrpcTgNotificationsService(
            ITelegramNotificationService telegramNotificationService,
            ILogger<GrpcTgNotificationsService> logger)
        {
            _telegramNotificationService = telegramNotificationService;
            _logger = logger;
        }


        /// <summary> gRPC-запрос на сообщение-уведомление для отправки в Telegram </summary>
        /// <param name="sendTgNotificationRequest"> gRPC-запрос | The request received from the client.</param>
        /// <param name="context"> gRPC-контекст | The context of the server-side call handler being invoked.</param>
        /// <returns>The response to send back to the client (wrapped by a task).</returns>
        [Authorize(Roles = "notificationsSender")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public override async Task<ResultReply> SendNotification(SendTgNotificationRequest sendTgNotificationRequest, ServerCallContext context)
        {
            var sendResult = await _telegramNotificationService.SendMessage(
                chatId: sendTgNotificationRequest.ChatId,
                text: sendTgNotificationRequest.Message);

            _logger.LogInformation("SENT NOTIFICATION. Tg.MessageID={ID}", sendResult.Id);

            return new ResultReply
            {
                MessageId = sendResult.Id,
                Message = "OK",
                StatusCode = (int)HttpStatusCode.OK
            };
        }
    }
}
