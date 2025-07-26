using System.Collections.Generic;
using ShopServices.Core.Enums;

namespace ShopServices.Core.Models
{
    /// <summary> Контактные данные для автоматических уведомлений о событиях </summary>
    public class ContactData
    {
        /// <summary> E-mail </summary>
        public string Email { get; }

        /// <summary> Телефон(ы) </summary>
        public string Phones { get; }

        /// <summary> Способы уведомлений по <see cref="ShopServices.Core.Enums.NotificationMethod"/></summary>
        public List<NotificationMethod> NotificationMethods { get; }

        public long? TelegramChatId { get; }
        
        public ContactData(
            string email,
            string phones,
            List<NotificationMethod> notificationMethods,
            long? telegramChatId)
        {
            Email = email;
            Phones = phones;
            NotificationMethods = notificationMethods;
            TelegramChatId = telegramChatId;
        }
    }
}
