using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;

namespace ShopServices.Core.Repositories
{
    public interface IEmailNotificationsRepository
    {
        public Task<Result> Create(Notification newNotification);
        public Task<Notification> GetEmailNotificationDataById(ulong notificationId);
        public Task<IEnumerable<Notification>> GetEmailNotificationsData(
            uint buyerId,
            uint? orderId,
            uint take,
            uint skipCount);
    }
}
