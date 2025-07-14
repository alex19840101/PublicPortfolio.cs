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

namespace NotificationsSender
{
    internal class NotificationWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly GrpcTgNotifications.GrpcTgNotificationsClient _grpcClient;
        //private readonly INotificationsSenderService _notificationsSenderService;
        private JwtSettings _jwtSettings;
        private readonly Serilog.ILogger _logger;
        private Metadata _headers;
        private ulong _minEmailNotificationId = 0;
        private ulong _minPhoneNotificationId = 0;

        public NotificationWorker(
            IServiceScopeFactory scopeFactory,
            GrpcTgNotifications.GrpcTgNotificationsClient grpcClient,
            //INotificationsSenderService notificationsSenderService,
            Serilog.ILogger logger)
        {
            _scopeFactory = scopeFactory;

            _grpcClient = grpcClient;
            //_notificationsSenderService = notificationsSenderService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
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

                    var phoneNotifications = await notificationsSenderService.GetPhoneNotificationsToSend(minNotificationId: _minPhoneNotificationId);
                    foreach (var pn in phoneNotifications)
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
                                Message = "Test notification to Telegram"
                            };
                            var resultReply = await _grpcClient.SendNotificationAsync(sendTgNotificationRequest, _headers, cancellationToken: stoppingToken);
                            if (resultReply.StatusCode != (int)HttpStatusCode.OK)
                            {
                                await phoneNotificationsRepository.SaveUnsuccessfulAttempt(pn.Id, DateTime.Now);
                                continue;
                            }

                            await phoneNotificationsRepository.UpdateSent(pn.Id, DateTime.Now);
                            continue;
                        }
                    }

                    var emailNotifications = await notificationsSenderService.GetPhoneNotificationsToSend(minNotificationId: _minPhoneNotificationId);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.ToString());
                }


            }
        }

        private JwtSecurityToken GetJwtSecurityToken()
        {
            var claims = new List<Claim>
            {
                //new Claim(ClaimTypes.Name, loginData.Login),
                new Claim(ClaimTypes.Role, Roles.NotificationsSender)
            };

            return new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromHours(24)),
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
