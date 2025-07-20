using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;

namespace ShopServices.Core.Repositories
{
    public interface IPhoneNotificationsRepository
    {
        public Task<Result> Create(Notification newNotification);
        public Task<Notification> GetPhoneNotificationDataById(ulong notificationId);
        public Task<IEnumerable<Notification>> GetPhoneNotificationsData(
            uint buyerId,
            uint? orderId,
            uint take,
            uint skipCount);

        public Task<Result> UpdateSent(
            ulong notificationId,
            DateTime sent);

        public Task<Result> SaveUnsuccessfulAttempt(
            ulong notificationId,
            DateTime lastUnsuccessfulAttempt);

        public Task<IEnumerable<Notification>> GetPhoneNotificationsToSend(
            ulong minNotificationId,
            uint take = 1000);
    }
}
