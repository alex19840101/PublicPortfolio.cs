using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using NotifierBySms.API.Interfaces;
using NotifierBySms.API.Services.gRPC.Notifications;
using System.Net;
using System;
using ShopServices.Core.Auth;
using ShopServices.Core;


namespace NotifierBySms.API.Services.gRPC
{
    /// <summary> Внутренний gRPC-сервис для работы с SMS-уведомлениями </summary>
    public class GrpcSmsNotificationsService : NotifierBySms.API.Services.gRPC.Notifications.GrpcSmsNotifications.GrpcSmsNotificationsBase
    {
        private readonly ISmsNotificationsService _smsNotificationsService;
        private readonly ILogger<GrpcSmsNotificationsService> _logger;

        /// <summary> Констуктор внутреннего gRPC-сервиса для работы с SMS-уведомлениями </summary>
        public GrpcSmsNotificationsService(
            ISmsNotificationsService SmsNotificationsService,
            ILogger<GrpcSmsNotificationsService> logger)
        {
            _smsNotificationsService = SmsNotificationsService;
            _logger = logger;
        }

        /// <summary> Обработка gRPC-запроса для отправки SMS-уведомления </summary>
        /// <param name="sendSmsNotificationRequest"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [Authorize(Roles = Roles.NotificationsSender)]
        [Authorize(AuthenticationSchemes = AuthSchemes.Bearer)]
        public override async Task<SendSmsReply> SendSmsNotification(SendSmsNotificationRequest sendSmsNotificationRequest, ServerCallContext context)
        {
            var expectedSecret = $"1{sendSmsNotificationRequest.PhoneSender}01{sendSmsNotificationRequest.PhoneReceiver}0";
            //$"1{pn.Sender}01{pn.Recipient}"
            if (string.IsNullOrWhiteSpace(sendSmsNotificationRequest.Secret) ||
                !string.Equals(sendSmsNotificationRequest.Secret, expectedSecret))
            {
                _logger.LogWarning("Secret mismatch. PhoneReceiver={PhoneReceiver}", sendSmsNotificationRequest.PhoneReceiver);

                return new SendSmsReply
                {
                    MessageId = 0,
                    Message = "Secret mismatch",
                    StatusCode = (int)HttpStatusCode.Forbidden
                };
            }

            var sendResult = await _smsNotificationsService.SendSmsNotification(
                phoneSender: sendSmsNotificationRequest.PhoneSender,
                phoneReceiver: sendSmsNotificationRequest.PhoneReceiver,
                message: sendSmsNotificationRequest.Message);

            if (sendResult == null)
            {
                _logger.LogWarning("sendResult == null. SMS.MessageID={ID}", sendResult?.Id);
                return new SendSmsReply
                {
                    Message = ResultMessager.SENDRESULT_IS_NULL_SMS,
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }

            _logger.LogInformation("SENT NOTIFICATION. SMS.MessageID={ID}", sendResult!.Id);

            return new SendSmsReply
            {
                MessageId = (long)sendResult.Id!,
                Message = "OK",
                StatusCode = (int)HttpStatusCode.OK
            };
        }
    }
}
