using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using NotifierByEmail.API.Interfaces;
using ShopServices.Abstractions;
using ShopServices.Core;

namespace NotifierByEmail.API.Services
{
    /// <summary> Нотификатор, отправляющий Email-уведомления </summary>
    public class EmailNotificationsService : IEmailNotificationsService
    {
        private readonly EmailBotOptionsSettings _emailBotOptionsSettings;
        private readonly ILogger<EmailNotificationsService> _logger;

        /// <summary> Констуктор нотификатора, отправляющего Email-уведомления </summary>
        public EmailNotificationsService(
            EmailBotOptionsSettings emailBotOptionsSettings,
            ILogger<EmailNotificationsService> logger)
        {
            _emailBotOptionsSettings = emailBotOptionsSettings;
            _logger = logger;
        }

        /// <summary> Отправка Email </summary>
        /// <param name="emailSender"> *E-mail-адрес отправителя </param>
        /// <param name="emailReceiver"> *E-mail-адрес получателя </param>
        /// <param name="topic"> *Тема сообщения (письма) </param>
        /// <param name="emailBody"> *Тело (Body) письма </param>
        public async Task<Result> SendEmail(
            string emailSender,
            string emailReceiver,
            string topic,
            string emailBody)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(emailSender, emailSender));
                message.To.Add(new MailboxAddress(emailReceiver, emailReceiver));
                message.Subject = topic;

                message.Body = new TextPart("plain")
                {
                    Text = emailBody
                };

                using var smtpClient = new SmtpClient();
                await smtpClient.ConnectAsync(
                    host: _emailBotOptionsSettings.HostName,
                    port: _emailBotOptionsSettings.Port,
                    useSsl: _emailBotOptionsSettings.UseSsl);

                // Note: only needed if the SMTP server requires authentication
                await smtpClient.AuthenticateAsync(
                    userName: _emailBotOptionsSettings.UserName,
                    password: _emailBotOptionsSettings.Password);

                var response = await smtpClient.SendAsync(message);
                await smtpClient.DisconnectAsync(true);

                return new Result
                {
                    Message = response,
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "ERROR SendEmail {emailReceiver}", emailReceiver);
                return new Result
                {
                    Message = ResultMessager.SEND_EMAIL_ERROR,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };
            }
        }
    }
}
