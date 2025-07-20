using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using ShopServices.Core.Auth;
using ShopServices.Core.Models;
using ShopServices.Core.Repositories;
using ShopServices.Core.Services;

namespace ShopServices.BusinessLogic
{
    public class NotificationsSenderService : INotificationsSenderService
    {
        private readonly IEmailNotificationsRepository _emailNotificationsRepository;
        private readonly IPhoneNotificationsRepository _phoneNotificationsRepository;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly string _key;

        public NotificationsSenderService(
            IEmailNotificationsRepository emailNotificationsRepository,
            IPhoneNotificationsRepository phoneNotificationsRepository,
            TokenValidationParameters tokenValidationParameters,
            string key)
        {
            _emailNotificationsRepository = emailNotificationsRepository;
            _phoneNotificationsRepository = phoneNotificationsRepository;
            _tokenValidationParameters = tokenValidationParameters;
            _key = key;
        }


        public JwtSettings GetJwtSettings() =>
            new JwtSettings
            {
                Audience = _tokenValidationParameters.ValidAudience,
                Issuer = _tokenValidationParameters.ValidIssuer,
                KEY = _key
            };

        public async Task<IEnumerable<Notification>> GetEmailNotificationsToSend(ulong minNotificationId,
            uint take = 1000)
        {
            return (await _emailNotificationsRepository.GetEmailNotificationsToSend(minNotificationId, take)).ToList();
        }
        public async Task<IEnumerable<Notification>> GetPhoneNotificationsToSend(ulong minNotificationId,
            uint take = 1000)
        {
            return (await _phoneNotificationsRepository.GetPhoneNotificationsToSend(minNotificationId, take)).ToList();
        }
    }
}
