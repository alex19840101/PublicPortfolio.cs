﻿using System;
using ShopServices.Core.Enums;

namespace ShopServices.Core.Models
{
    /// <summary> Класс однократного E-mail-уведомления </summary>
    public class EmailNotification : Notification
    {
        public EmailNotification(
            ulong id,
            NotificationMethod notificationMethod,
            ModelEntityType modelEntityType,
            uint? buyerId,
            ulong changedEntityId,
            string sender,
            string recipient,
            string topic,
            string message,
            DateTime created,
            string creator,
            DateTime? sent = null,
            uint unsuccessfulAttempts = 0,
            DateTime? lastUnsuccessfulAttempt = null) : base(
                id,
                notificationMethod,
                modelEntityType,
                buyerId,
                changedEntityId,
                sender,
                recipient,
                topic,
                message,
                created,
                creator,
                sent,
                unsuccessfulAttempts,
                lastUnsuccessfulAttempt)
        {
        }
    }
}
