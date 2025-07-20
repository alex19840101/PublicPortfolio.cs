using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core;
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

        public async Task<Result> AddNotification(Notification newNotification)
        {
            var errorResult = UnValidatedNotificationResult(newNotification, checkNotificationId: false);
            if (errorResult != null)
                return errorResult;

            var createResult = newNotification.NotificationMethod == Core.Enums.NotificationMethod.Email ?
                await _emailNotificationsRepository.Create(newNotification) :
                await _phoneNotificationsRepository.Create(newNotification);

            return createResult;
        }

        public async Task<Notification> GetEmailNotificationDataById(ulong notificationId)
        {
            return await _emailNotificationsRepository.GetEmailNotificationDataById(notificationId);
        }

        public async Task<IEnumerable<Notification>> GetNotifications(
            uint buyerId,
            uint? orderId,
            uint byPage = 10,
            uint page = 1)
        {
            var take = byPage;
            var skip = page > 1 ? (page - 1) * byPage : 0;

            var emailNotifications = (await _emailNotificationsRepository.GetEmailNotificationsData(
                buyerId: buyerId,
                orderId: orderId,
                take: take,
                skipCount: skip)).ToList();

            var notifications = emailNotifications;

            var phoneNotifications = (await _phoneNotificationsRepository.GetPhoneNotificationsData(
                buyerId: buyerId,
                orderId: orderId,
                take: take,
                skipCount: skip)).ToList();

            notifications.AddRange(phoneNotifications);

            return notifications?.OrderBy(n => n.Created).Take((int)take).Skip((int)skip);
        }

        public async Task<Notification> GetPhoneNotificationDataById(ulong notificationId)
        {
            return await _phoneNotificationsRepository.GetPhoneNotificationDataById(notificationId);
        }

        /// <summary> Валидация данных уведомления </summary>
        /// <param name="notification"> Данные уведомления </param>
        /// <param name="checkNotificationId"> Проверять ли, что notification.Id > 0 </param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private Result UnValidatedNotificationResult(Notification notification, bool checkNotificationId = true)
        {
            if (notification == null)
                throw new ArgumentNullException(ResultMessager.NOTIFICATION_RARAM_NAME);

            if (checkNotificationId && notification.Id == 0)
                return new Result(ResultMessager.NOTIFICATION_ID_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.BadRequest);

            if (notification.ChangedEntityId == 0)
                return new Result(ResultMessager.CHANGED_ENTITY_ID_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.BadRequest);

            if (notification.BuyerId == 0)
                return new Result(ResultMessager.BUYER_NOT_FOUND, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(notification.Sender))
                return new Result(ResultMessager.NOTIFICATION_SENDER_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(notification.Recipient))
                return new Result(ResultMessager.NOTIFICATION_RECIPIENT_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(notification.Message))
                return new Result(ResultMessager.NOTIFICATION_MESSAGE_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(notification.Topic))
                return new Result(ResultMessager.NOTIFICATION_TOPIC_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(notification.Creator))
                return new Result(ResultMessager.NOTIFICATION_CREATOR_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (notification.Created > DateTime.Now)
                return new Result(ResultMessager.NOTIFICATION_CREATED_IN_FUTURE, System.Net.HttpStatusCode.BadRequest);

            if (notification.Sent != null)
                return new Result(ResultMessager.NOTIFICATION_SENT_SHOULD_BE_NULL, System.Net.HttpStatusCode.BadRequest);

            if (notification.LastUnsuccessfulAttempt != null)
                return new Result(ResultMessager.NOTIFICATION_LAST_ATTEMPT_SHOULD_BE_NULL, System.Net.HttpStatusCode.BadRequest);
            
            if (notification.UnsuccessfulAttempts != 0)
                return new Result(ResultMessager.NOTIFICATION_LAST_ATTEMPT_SHOULD_BE_NULL, System.Net.HttpStatusCode.BadRequest);


            return null;
        }
    }
}
