using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TelegramBot.API.Interfaces;
using TelegramBot.API.Services.gRPC.Notifications;
using System.Net;
using System;
using ShopServices.Core.Auth;
using ShopServices.Core;

namespace TelegramBot.API.Services.gRPC
{
    /// <summary> Внутренний gRPC-сервис для работы с Telegram-уведомлениями </summary>
    public class GrpcTgNotificationsService : TelegramBot.API.Services.gRPC.Notifications.GrpcTgNotifications.GrpcTgNotificationsBase
    {
        private readonly ITelegramNotificationService _telegramNotificationService;
        private readonly ILogger<GrpcTgNotificationsService> _logger;
        private readonly string _secret;

        /// <summary> Констуктор внутреннего gRPC-сервиса для работы с Telegram-уведомлениями </summary>
        public GrpcTgNotificationsService(
            ITelegramNotificationService telegramNotificationService,
            ILogger<GrpcTgNotificationsService> logger,
            string secret)
        {
            _telegramNotificationService = telegramNotificationService;
            _logger = logger;
            _secret = secret;
        }


        /// <summary> Обработка gRPC-запроса  для отправки уведомления в Telegram </summary>
        /// <param name="sendTgNotificationRequest"> gRPC-запрос | The request received from the client.</param>
        /// <param name="context"> gRPC-контекст | The context of the server-side call handler being invoked.</param>
        /// <returns>The response to send back to the client (wrapped by a task).</returns>
        [Authorize(Roles = Roles.NotificationsSender)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public override async Task<ResultReply> SendNotification(SendTgNotificationRequest sendTgNotificationRequest, ServerCallContext context)
        {
            var expectedSecret = $"{sendTgNotificationRequest.ChatId.GetHashCode()}{_secret}{sendTgNotificationRequest.Message.GetHashCode()}";
            if (string.IsNullOrWhiteSpace(sendTgNotificationRequest.Secret) ||
                !string.Equals(sendTgNotificationRequest.Secret, expectedSecret))
            {
                _logger.LogWarning("Secret mismatch. Tg.ChatID={ID}", sendTgNotificationRequest.ChatId);

                return new ResultReply
                {
                    MessageId = 0,
                    Message = "Secret mismatch",
                    StatusCode = (int)HttpStatusCode.Forbidden
                };
            }
            
            var sendResult = await _telegramNotificationService.SendMessage(
                chatId: sendTgNotificationRequest.ChatId,
                text: sendTgNotificationRequest.Message);

            if (sendResult == null)
            {
                _logger.LogWarning("sendResult == null. Tg.MessageID={ID}", sendResult?.Id);
                return new ResultReply
                {
                    Message = ResultMessager.SENDRESULT_IS_NULL_TELEGRAM,
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }

            _logger.LogInformation("SENT NOTIFICATION. Tg.MessageID={ID}", sendResult!.Id);

            return new ResultReply
            {
                MessageId = sendResult.Id,
                Message = "OK",
                StatusCode = (int)HttpStatusCode.OK
            };
        }
    }
}
