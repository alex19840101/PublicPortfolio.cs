using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using Azure.Communication.Sms;
using Microsoft.Extensions.Logging;
using NotifierBySms.API.Interfaces;
using ShopServices.Abstractions;
using ShopServices.Core;

namespace NotifierBySms.API.Services
{
    /// <summary> Нотификатор, отправляющий SMS-уведомления через Azure </summary>
    public class SmsNotificationsByAzureService : ISmsNotificationsService
    {
        private readonly SmsBotClientOptionsSettings _smsBotClientOptionsSettings;
        private readonly ILogger<SmsNotificationsByAzureService> _logger;

        /// <summary> Констуктор нотификатора, отправляющего SMS-уведомления </summary>
        public SmsNotificationsByAzureService(
            SmsBotClientOptionsSettings smsBotClientOptionsSettings,
            ILogger<SmsNotificationsByAzureService> logger)
        {
            _smsBotClientOptionsSettings = smsBotClientOptionsSettings;
            _logger = logger;
        }

        /// <summary> Отправка SMS </summary>
        /// <param name="phoneSender"> *Телефон отправителя </param>
        /// <param name="phoneReceiver"> *Телефон получателя </param>
        /// <param name="message"> *Сообщение </param>
        public async Task<Result> SendSmsNotification(
            string phoneSender,
            string phoneReceiver,
            string message)
        {
            try
            {
                var connectionString = _smsBotClientOptionsSettings.ConnectionString!;
                SmsClient smsClient = new SmsClient(connectionString);

                SmsSendResult sendResult = await smsClient.SendAsync(
                    from: phoneSender, // E.164 formatted from phone number used to send SMS
                    to: phoneReceiver, // E.164 formatted recipient phone number
                    message: message,
                    options: new SmsSendOptions(enableDeliveryReport: true)
                    {
                        Tag = "notification"
                    });

                if (!sendResult.Successful)
                {
                    _logger.LogWarning("{status}|{errorMessage}|SendSmsNotification {phoneReceiver}", sendResult.HttpStatusCode, sendResult.ErrorMessage, phoneReceiver);
                    return new Result
                    {
                        Message = ResultMessager.SEND_SMS_ERROR,
                        StatusCode = System.Net.HttpStatusCode.InternalServerError
                    };
                }
                
                _logger.LogInformation("{status}|{messageId}|SendSmsNotification {phoneReceiver}", sendResult.HttpStatusCode, sendResult.MessageId, phoneReceiver);
                return new Result
                {
                    Message = ResultMessager.OK,
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "ERROR SendSmsNotification {phoneReceiver}", phoneReceiver);
                return new Result
                {
                    Message = ResultMessager.SEND_SMS_ERROR,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };
            }
        }
    }
}
