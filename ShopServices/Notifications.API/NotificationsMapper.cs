using System;
using System.Collections.Generic;
using System.Linq;
using Notifications.API.Contracts.Requests;
using Notifications.API.Contracts.Responses;
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
        /// <returns></returns>
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
