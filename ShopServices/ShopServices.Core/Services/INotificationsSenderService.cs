using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Core.Auth;
using ShopServices.Core.Models;

namespace ShopServices.Core.Services
{
    public interface INotificationsSenderService
    {
        public JwtSettings GetJwtSettings();
        public Task<IEnumerable<Notification>> GetEmailNotificationsToSend(ulong minNotificationId, uint take = 1000);
        public Task<IEnumerable<Notification>> GetPhoneNotificationsToSend(ulong minNotificationId, uint take = 1000);
    }
}
