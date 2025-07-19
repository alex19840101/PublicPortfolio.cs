using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NotifierBySms.API.Interfaces;
using ShopServices.Abstractions;
using ShopServices.Core;

namespace NotifierBySms.API.Services
{
    /// <summary> Нотификатор, отправляющий SMS-уведомления </summary>
    public class SmsNotificationsService : ISmsNotificationsService
    {
        private readonly SmsBotClientOptionsSettings _smsBotClientOptionsSettings;
        private readonly ILogger<SmsNotificationsService> _logger;

        /// <summary> Констуктор нотификатора, отправляющего SMS-уведомления </summary>
        public SmsNotificationsService(
            SmsBotClientOptionsSettings smsBotClientOptionsSettings,
            ILogger<SmsNotificationsService> logger)
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
                //throw new NotImplementedException("Payed SMS are not implemented in pet project");
                _logger.LogInformation("SendSmsNotification. Simulation SMS to {phoneReceiver} => OK", phoneReceiver);
                return new Result
                {
                    Id = 100500,
                    Message = ResultMessager.SMS_OK_SIMULATION,
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
