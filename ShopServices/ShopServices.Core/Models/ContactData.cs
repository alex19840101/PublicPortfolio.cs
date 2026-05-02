using System.Collections.Generic;
using System.Linq;
using ShopServices.Core.Enums;

namespace ShopServices.Core.Models
{
    /// <summary> Контактные данные для автоматических уведомлений о событиях </summary>
    public class ContactData
    {
        /// <summary> E-mail </summary>
        public string Email { get; }

        /// <summary> Телефон(ы) </summary>
        public string Phone { get; }

        /// <summary> Способы уведомлений по <see cref="ShopServices.Core.Enums.NotificationMethod"/></summary>
        public System.Int16[] NotificationMethods { get; }      //Int16 для Dapper

        public long? TelegramChatId { get; }

        /// <summary> Контактные данные для автоматических уведомлений о событиях </summary>
        public ContactData(
            string email,
            string phone,
            System.Int16[] notificationMethods,
            long? telegramChatId)
        {
            Email = email;
            Phone = phone;
            NotificationMethods = notificationMethods;
            TelegramChatId = telegramChatId;
        }

        public List<NotificationMethod> GetNotificationMethods() => NotificationMethods.Select(nm => (NotificationMethod)nm).ToList();

        /// <summary> Контактные данные для автоматических уведомлений о событиях </summary>
        public ContactData()
        { }
    }
}
