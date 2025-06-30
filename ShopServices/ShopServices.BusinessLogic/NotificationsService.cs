using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;
using ShopServices.Core.Repositories;
using ShopServices.Core.Services;

namespace ShopServices.BusinessLogic
{
    public class NotificationsService : INotificationsService
    {
        private readonly IEmailNotificationsRepository _emailNotificationsRepository;
        private readonly IPhoneNotificationsRepository _phoneNotificationsRepository;

        public NotificationsService(
            IEmailNotificationsRepository emailNotificationsRepository,
            IPhoneNotificationsRepository phoneNotificationsRepository)
        {
            _emailNotificationsRepository = emailNotificationsRepository;
            _phoneNotificationsRepository = phoneNotificationsRepository;
        }

        public async Task<Result> AddNotification(Notification value)
        {
            throw new NotImplementedException();
        }

        public async Task<Notification> GetEmailNotificationDataById(ulong notificationId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Notification>> GetNotifications(uint buyerId, uint? orderId, uint byPage, uint page)
        {
            throw new NotImplementedException();
        }

        public async Task<Notification> GetPhoneNotificationDataById(ulong notificationId)
        {
            throw new NotImplementedException();
        }
    }
}
