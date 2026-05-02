using System.Collections.Generic;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;

namespace ShopServices.Core.Services
{
    public interface INotificationsService
    {
        public Task<Result> AddNotification(Notification notification);
        public Task<Notification> GetEmailNotificationDataById(ulong notificationId);
        public Task<IEnumerable<Notification>> GetNotifications(uint buyerId, uint? orderId, uint byPage, uint page);
        public Task<Notification> GetPhoneNotificationDataById(ulong notificationId);
    }
}
