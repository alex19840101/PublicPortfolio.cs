using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using NotifierByEmail.API.Interfaces;
using NotifierByEmail.API.Services.gRPC.Notifications;
using System.Net;
using System;
using ShopServices.Core.Auth;
using ShopServices.Core;


namespace NotifierByEmail.API.Services.gRPC
{
    /// <summary> Внутренний gRPC-сервис для работы с E-mail-уведомлениями </summary>
    public class GrpcEmailNotificationsService : NotifierByEmail.API.Services.gRPC.Notifications.GrpcEmailNotifications.GrpcEmailNotificationsBase
    {
        private readonly IEmailNotificationsService _emailNotificationsService;
        private readonly ILogger<GrpcEmailNotificationsService> _logger;
        private readonly string _secret;

        /// <summary> Констуктор внутреннего gRPC-сервиса для работы с E-mail-уведомлениями </summary>
        public GrpcEmailNotificationsService(
            IEmailNotificationsService emailNotificationsService,
            ILogger<GrpcEmailNotificationsService> logger,
            string secret)
        {
            _emailNotificationsService = emailNotificationsService;
            _logger = logger;
            _secret = secret;
        }

        /// <summary> Обработка gRPC-запроса для отправки E-mail-письма </summary>
        /// <param name="sendEmailNotificationRequest"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [Authorize(Roles = Roles.NotificationsSender)]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public override async Task<SendEmailReply> SendEmailNotification(SendEmailNotificationRequest sendEmailNotificationRequest, ServerCallContext context)
        {
            var expectedSecret = $"{sendEmailNotificationRequest.EmailReceiver.GetHashCode()}{_secret}{sendEmailNotificationRequest.Topic.GetHashCode()}";
            if (string.IsNullOrWhiteSpace(sendEmailNotificationRequest.Secret) ||
                !string.Equals(sendEmailNotificationRequest.Secret, expectedSecret))
            {
                _logger.LogWarning("Secret mismatch. EmailReceiver={EmailReceiver}", sendEmailNotificationRequest.EmailReceiver);

                return new SendEmailReply
                {
                    MessageId = 0,
                    Message = "Secret mismatch",
                    StatusCode = (int)HttpStatusCode.Forbidden
                };
            }

            var sendResult = await _emailNotificationsService.SendEmail(
                emailSender: sendEmailNotificationRequest.EmailSender,
                emailReceiver: sendEmailNotificationRequest.EmailReceiver,
                topic: sendEmailNotificationRequest.Topic,
                emailBody: sendEmailNotificationRequest.EmailBody);

            if (sendResult == null)
            {
                _logger.LogWarning("sendResult == null. Tg.MessageID={ID}", sendResult?.Id);
                return new SendEmailReply
                {
                    Message = ResultMessager.SENDRESULT_IS_NULL_EMAIL,
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }

            _logger.LogInformation("SENT NOTIFICATION. Tg.MessageID={ID}", sendResult!.Id);

            return new SendEmailReply
            {
                MessageId = (long)sendResult.Id!,
                Message = "OK",
                StatusCode = (int)HttpStatusCode.OK
            };
        }
    }
}
