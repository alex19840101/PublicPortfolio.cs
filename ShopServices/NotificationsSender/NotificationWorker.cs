using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog.Events;
using ShopServices.Core.Auth;
using ShopServices.Core.Repositories;
using ShopServices.Core.Services;
using ShopServices.DataAccess;
using TelegramBot.API.Services.gRPC.Notifications;
using NotifierByEmail.API.Services.gRPC.Notifications;
using NotifierBySms.API.Services.gRPC.Notifications;
using System.Linq;

namespace NotificationsSender
{
    internal class NotificationWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly GrpcTgNotifications.GrpcTgNotificationsClient _grpcTgClient;
        private readonly GrpcEmailNotifications.GrpcEmailNotificationsClient _grpcEmailClient;
        private readonly GrpcSmsNotifications.GrpcSmsNotificationsClient _grpcSmsClient;

        #region //INotificationsSenderService _notificationsSenderService, dbContext, phoneNotificationsRepository, emailNotificationsRepository получаем из DI и IServiceScopeFactory
        //private readonly INotificationsSenderService _notificationsSenderService;
        #endregion //INotificationsSenderService _notificationsSenderService, dbContext, phoneNotificationsRepository, emailNotificationsRepository получаем из DI и IServiceScopeFactory
        
        private JwtSettings? _jwtSettings;
        private readonly Serilog.ILogger _logger;
        private Metadata? _headers;
        private DateTime? _tokenExpires = null;
        private ulong _minEmailNotificationId = 0;
        private ulong _minPhoneNotificationId = 0;
        private const ulong SENDING_INTERVAL_SECONDS = 60;
        private const ulong TOKEN_EXPIRATION_TIME_HOURS = 1;

        public NotificationWorker(
            IServiceScopeFactory scopeFactory,
            GrpcTgNotifications.GrpcTgNotificationsClient grpcClient,
            GrpcEmailNotifications.GrpcEmailNotificationsClient grpcEmailClient,
            GrpcSmsNotifications.GrpcSmsNotificationsClient grpcSmsClient,
            Serilog.ILogger logger)
        {
            _scopeFactory = scopeFactory;

            _grpcTgClient = grpcClient;
            _grpcEmailClient = grpcEmailClient;
            _grpcSmsClient = grpcSmsClient;

            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Worker running at: {time}", DateTimeOffset.Now);

            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    if (_logger.IsEnabled(LogEventLevel.Information))
                    {
                        _logger.Information("Worker running at: {time}", DateTimeOffset.Now);
                    }

                    await using var scope = _scopeFactory.CreateAsyncScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<ShopServicesDbContext>();
                    var phoneNotificationsRepository = scope.ServiceProvider.GetRequiredService<IPhoneNotificationsRepository>();
                    var emailNotificationsRepository = scope.ServiceProvider.GetRequiredService<IEmailNotificationsRepository>();
                    var notificationsSenderService = scope.ServiceProvider.GetRequiredService<INotificationsSenderService>();

                    if (_headers == null)
                    {
                        _jwtSettings = notificationsSenderService.GetJwtSettings();
                        DefineHeaders();
                    }
                    if (_tokenExpires < DateTime.UtcNow)
                        DefineHeaders();

                    var phoneNotifications = await notificationsSenderService.GetPhoneNotificationsToSend(minNotificationId: _minPhoneNotificationId);
                    await SendPhoneNotifications(phoneNotificationsRepository, phoneNotifications, cancellationToken);

                    var emailNotifications = await notificationsSenderService.GetEmailNotificationsToSend(minNotificationId: _minEmailNotificationId);
                    await SendEmailNotifications(emailNotificationsRepository, emailNotifications, cancellationToken);

                    #if DEBUG
                    Console.WriteLine($"{nameof(_jwtSettings)}: {_jwtSettings!.ToString()}");
                    #endif
                    _logger.Information("Worker pause at {time}", DateTimeOffset.Now);
                    await Task.Delay(TimeSpan.FromSeconds(SENDING_INTERVAL_SECONDS), cancellationToken);

                }
                catch (Exception ex)
                {
                    _logger.Error(ex.ToString());
                }
            }
            _logger.Information("Cancellation at: {time}", DateTimeOffset.Now);
            return;
        }

        private async Task SendEmailNotifications(
            IEmailNotificationsRepository emailNotificationsRepository,
            IEnumerable<ShopServices.Core.Models.Notification> emailNotifications,
            CancellationToken cancellationToken)
        {
            var noError = true;
            var sentCount = 0;
            var errorsCount = 0;
            foreach (var en in emailNotifications)
            {
                try
                {
                    var sendEmailNotificationRequest = new SendEmailNotificationRequest
                    {
                        EmailSender = en.Sender,
                        EmailReceiver = en.Recipient,
                        Topic = en.Topic,
                        EmailBody = en.Message,
                        Secret = $"1{en.Recipient.GetHashCode()}00{en.Topic.GetHashCode()}"
                    };
                    var resultReply = await _grpcEmailClient.SendEmailNotificationAsync(sendEmailNotificationRequest, _headers, cancellationToken: cancellationToken);
                    if (resultReply.StatusCode != (int)HttpStatusCode.OK)
                    {
                        await emailNotificationsRepository.SaveUnsuccessfulAttempt(en.Id, DateTime.Now);
                        noError = false;
                        errorsCount++;
                        continue;
                    }

                    await emailNotificationsRepository.UpdateSent(en.Id, DateTime.Now);
                    if (noError)
                        _minEmailNotificationId = en.Id + 1;
                    continue;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"en.Id={en.Id}");
                    await emailNotificationsRepository.SaveUnsuccessfulAttempt(en.Id, DateTime.Now);
                    noError = false;
                    errorsCount++;
                }
            }
            var count = emailNotifications.Count();
            Console.WriteLine($"{nameof(SendEmailNotifications)}\t|count:{count}|sentCount:{sentCount}|errorsCount:{errorsCount}");
        }

        private async Task SendPhoneNotifications(
            IPhoneNotificationsRepository phoneNotificationsRepository,
            IEnumerable<ShopServices.Core.Models.Notification> phoneNotifications,
            CancellationToken cancellationToken)
        {
            var noError = true;
            
            var sentTgCount = 0;
            var errorsTgCount = 0;
            var sentSmsCount = 0;
            var errorsSmsCount = 0;
            foreach (var pn in phoneNotifications)
            {
                try
                {
                    if (pn.NotificationMethod == ShopServices.Core.Enums.NotificationMethod.TelegramMessage)
                    {
                        var chatIdParsed = long.TryParse(pn.Recipient, out long chatId);
                        if (!chatIdParsed)
                        {
                            _logger.Warning("{Recipient} is not long chatId", pn.Recipient);
                            await phoneNotificationsRepository.SaveUnsuccessfulAttempt(pn.Id, DateTime.Now);
                            continue;
                        }
                        var sendTgNotificationRequest = new SendTgNotificationRequest
                        {
                            ChatId = chatId,
                            Message = pn.Message,
                            Secret = $"1{chatId.GetHashCode()}0"
                        };
                        var resultReply = await _grpcTgClient.SendNotificationAsync(sendTgNotificationRequest, _headers, cancellationToken: cancellationToken);
                        if (resultReply.StatusCode != (int)HttpStatusCode.OK)
                        {
                            await phoneNotificationsRepository.SaveUnsuccessfulAttempt(pn.Id, DateTime.Now);
                            noError = false;
                            errorsTgCount++;
                            continue;
                        }

                        await phoneNotificationsRepository.UpdateSent(pn.Id, DateTime.Now);

                        if (noError)
                            _minPhoneNotificationId = pn.Id + 1;
                        sentTgCount++;
                        continue;
                    }
                    //SMS
                    var sendSmsNotificationRequest = new SendSmsNotificationRequest
                    {
                        PhoneSender = pn.Sender,
                        PhoneReceiver = pn.Recipient,
                        Message = pn.Message
                    };
                    var smsResultReply = await _grpcSmsClient.SendSmsNotificationAsync(sendSmsNotificationRequest, _headers, cancellationToken: cancellationToken);
                    if (smsResultReply.StatusCode != (int)HttpStatusCode.OK)
                    {
                        await phoneNotificationsRepository.SaveUnsuccessfulAttempt(pn.Id, DateTime.Now);
                        noError = false;
                        errorsSmsCount++;
                        continue;
                    }

                    await phoneNotificationsRepository.UpdateSent(pn.Id, DateTime.Now);
                    if (noError)
                        _minPhoneNotificationId = pn.Id + 1;
                    sentSmsCount++;
                    continue;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"pn.Id={pn.Id}");
                    await phoneNotificationsRepository.SaveUnsuccessfulAttempt(pn.Id, DateTime.Now);
                    noError = false;
                    if (pn.NotificationMethod == ShopServices.Core.Enums.NotificationMethod.TelegramMessage)
                        errorsTgCount++;
                    else errorsSmsCount++;
                }
            }
            var count = phoneNotifications.Count();
            var tgCount = phoneNotifications.Count(pn => pn.NotificationMethod == ShopServices.Core.Enums.NotificationMethod.TelegramMessage);
            var smsCount = count - tgCount;
            Console.WriteLine($"{nameof(SendPhoneNotifications)}\t|Tg\t|count:{tgCount}|sentCount:{sentTgCount}|errorsCount:{errorsTgCount}");
            Console.WriteLine($"{nameof(SendPhoneNotifications)}\t|SMS\t|count:{smsCount}|sentCount:{sentSmsCount}|errorsCount:{errorsSmsCount}");
        }

        private JwtSecurityToken GetJwtSecurityToken()
        {
            var claims = new List<Claim>
            {
                //new Claim(ClaimTypes.Name, loginData.Login),
                new Claim(ClaimTypes.Role, Roles.NotificationsSender)
            };
            _tokenExpires = DateTime.UtcNow.Add(TimeSpan.FromHours(TOKEN_EXPIRATION_TIME_HOURS));

            return new JwtSecurityToken(
                issuer: _jwtSettings!.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: _tokenExpires,
                signingCredentials: new SigningCredentials(
                    key: new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(_jwtSettings.KEY!)),
                    algorithm: SecurityAlgorithms.HmacSha256));
        }

        /// <summary>
        /// Установка заголовка(ов) для gRPC-авторизации
        /// </summary>
        private void DefineHeaders()
        {
            var jwt = GetJwtSecurityToken();

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            _headers = RequestHeadersPreparator.GetMetadataWithAuthorizationHeader(token)!;
        }
    }
}
