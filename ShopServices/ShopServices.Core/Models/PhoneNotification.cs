using System;
using ShopServices.Core.Enums;

namespace ShopServices.Core.Models
{
    /// <summary> Класс однократного уведомления по телефону (SMS/Telegram) </summary>
    public class PhoneNotification : Notification
    {
        public PhoneNotification(
            ulong id,
            NotificationMethod notificationMethod,
            ModelEntityType modelEntityType,
            uint? buyerId,
            ulong changedEntityId,
            string sender,
            string recipient,
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
