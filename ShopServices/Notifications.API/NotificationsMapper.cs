using System;
using System.Collections.Generic;
using System.Linq;
using Notifications.API.Contracts.Requests;
using Notifications.API.Contracts.Responses;
using ShopServices.Core;
using ShopServices.Core.Enums;
using ShopServices.Core.Models;

namespace Notifications.API
{
    internal static class NotificationsMapper
    {
        internal static NotificationDataResponseDto GetNotificationDataResponseDto(Notification coreNotification)
        {
            return new Contracts.Responses.NotificationDataResponseDto
            {
                Id = coreNotification.Id,
                ModelEntityType = coreNotification.ModelEntityType,
                ChangedEntityId = coreNotification.ChangedEntityId,
                BuyerId = coreNotification.BuyerId,
                NotificationMethod = coreNotification.NotificationMethod,
                From = coreNotification.Sender,
                To = coreNotification.Recipient,
                Message = coreNotification.Message,
                Topic = coreNotification.Topic,
                Created = coreNotification.Created,
                Creator = coreNotification.Creator,
                Sent = coreNotification.Sent,
                UnsuccessfulAttempts = coreNotification.UnsuccessfulAttempts,
                LastUnsuccessfulAttempt = coreNotification.LastUnsuccessfulAttempt
            };
        }

        /// <summary>
        /// Маппер-формирователь Core.Models.Notification-модели нового уведомления
        /// </summary>
        /// <param name="addNotificationRequestDto"></param>
        /// <param name="employeeId"> Id работника </param>
        /// <returns> Core.Models.Notification-модель нового уведомления </returns>
        internal static Notification PrepareCoreNotification(
            AddNotificationRequestDto addNotificationRequestDto,
            uint employeeId)
        {
            if (addNotificationRequestDto.NotificationMethod != ShopServices.Core.Enums.NotificationMethod.Email)
                addNotificationRequestDto.Message = $"{addNotificationRequestDto.Topic}.{addNotificationRequestDto.Message}";

            return new Notification(
                id: 0,
                notificationMethod: addNotificationRequestDto.NotificationMethod,
                modelEntityType: addNotificationRequestDto.ModelEntityType,
                buyerId: addNotificationRequestDto.BuyerId,
                changedEntityId: addNotificationRequestDto.ChangedEntityId,
                sender: addNotificationRequestDto.From,
                recipient: addNotificationRequestDto.To,
                topic: addNotificationRequestDto.Topic,
                message: addNotificationRequestDto.Message,
                created: DateTime.Now,
                creator: $"Employee {employeeId} Notifications.API");
        }

        /// <summary> Маппер-формирователь Core.Models.Notification-модели нового уведомления </summary>
        /// <returns> Core.Models.Notification-модель нового уведомления | null - не уведомлять </returns>
        internal static Notification? PrepareCoreNotification(
            ContactData contactData,
            NotificationMethod notificationMethod,
            ModelEntityType modelEntityType,
            ulong changedEntityId,
            string notification,
            uint? buyerId)
        {
            string sender;
            string recipient;
            switch (notificationMethod)
            {
                case NotificationMethod.Email:
                    sender = NotificationMessages.EMAIL_FROM;
                    recipient = contactData.Email;
                    break;

                case NotificationMethod.TelegramMessage:
                    sender = NotificationMessages.TELEGRAM_NOTIFICATION_FROM;
                    recipient = contactData.TelegramChatId!.Value.ToString();
                    break;

                case NotificationMethod.SMS:
                    sender = NotificationMessages.SMS_FROM;
                    recipient = contactData.Phone.Split(",")[0];
                    break;

                default:
                    return null;
            }
            if (string.IsNullOrWhiteSpace(recipient))
            {
                return null;
            }

            return new Notification(
                id: 0,
                notificationMethod: notificationMethod,
                modelEntityType: modelEntityType,
                buyerId: buyerId,
                changedEntityId: changedEntityId,
                sender: sender,
                recipient: recipient,
                topic: notification,
                message: notification,
                created: DateTime.Now,
                creator: NotificationMessages.NOTIFICATION_API);
        }

        /// <summary>
        /// Маппинг IEnumerable(Core.Models.Notification) - IEnumerable(Contracts.Responses.NotificationDataResponseDto)
        /// </summary>
        /// <param name="notificationsList"> список уведомлений IEnumerable(Core.Models.Notification) </param>
        /// <returns></returns>
        internal static IEnumerable<Contracts.Responses.NotificationDataResponseDto> GetNotificationsDtos(this IEnumerable<Notification> notificationsList)
        {
            return notificationsList.Select(coreNotification => new Contracts.Responses.NotificationDataResponseDto
            {
                Id = coreNotification.Id,
                ModelEntityType = coreNotification.ModelEntityType,
                ChangedEntityId = coreNotification.ChangedEntityId,
                BuyerId = coreNotification.BuyerId,
                NotificationMethod = coreNotification.NotificationMethod,
                From = coreNotification.Sender,
                To = coreNotification.Recipient,
                Message = coreNotification.Message,
                Topic = coreNotification.Topic,
                Created = coreNotification.Created,
                Creator = coreNotification.Creator,
                Sent = coreNotification.Sent,
                UnsuccessfulAttempts = coreNotification.UnsuccessfulAttempts,
                LastUnsuccessfulAttempt = coreNotification.LastUnsuccessfulAttempt
            });
        }
    }
}
