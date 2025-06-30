using System;
using ShopServices.Core.Enums;

namespace ShopServices.Core.Models
{
    /// <summary> Класс однократного уведомления </summary>
    public class Notification
    {
        public ulong Id { get; private set; }

        /// <summary> *Метод уведомления по <see cref="ShopServices.Core.Enums.NotificationMethod"/> </summary>
        public NotificationMethod NotificationMethod { get; private set; }

        /// <summary> Тип измененной модели (сущности) (для классификации уведомлений) по <see cref="Enums.ModelEntityType"/> </summary>
        public ModelEntityType ModelEntityType { get; private set; }

        /// <summary> Id измененной сущности в БД </summary>
        public ulong ChangedEntityId { get; private set; }

        /// <summary> Уникальный идентификатор покупателя </summary>
        public uint? BuyerId { get; private set; }

        /// <summary> Отправитель уведомления (сообщения) (E-mail/телефон) </summary>
        public string Sender { get; private set; }

        /// <summary> Получатель уведомления (сообщения) (E-mail/телефон) </summary>
        public string Recipient { get; private set; }
        public string Message { get; private set; }
        
        /// <summary> Тема сообщения (в E-mail - отдельно, в SMS/Telegram-сообщении - в начале сообщения) </summary>
        public string Topic { get; private set; }

        /// <summary> Дата и время создания уведомления </summary>
        public DateTime Created { get; private set; }

        /// <summary> Создатель (автор) уведомления </summary>
        public string Creator { get; private set; }

        /// <summary> Дата и время отправки уведомления </summary>
        public DateTime? Sent { get; set; }
        
        /// <summary> Количество сделанных неудачных попыток отправки уведомления </summary>
        public uint UnsuccessfulAttempts { get; private set; } = 0;

        /// <summary> Дата и время последней неудачной отправки уведомления </summary>
        public DateTime? LastUnsuccessfulAttempt { get; set; }

        /// <summary> ShopServices.Core.Models.Notification - конструктор уведомления </summary>
        /// <param name="id"> Id однократного уведомления в хранилище Email- или телефонных уведомлений </param>
        /// <param name="notificationMethod"> *Метод уведомления по <see cref="ShopServices.Core.Enums.NotificationMethod"/> </param>
        /// <param name="modelEntityType"> Тип измененной модели (сущности) (для классификации уведомлений) по <see cref="Enums.ModelEntityType"/> </param>
        /// <param name="buyerId"> Уникальный идентификатор покупателя </param>
        /// <param name="changedEntityId"> Id измененной сущности в БД </param>
        /// <param name="sender"> Отправитель уведомления (сообщения) (E-mail/телефон) </param>
        /// <param name="recipient"> Получатель уведомления (сообщения) (E-mail/телефон) </param>
        /// <param name="topic"> Тема уведомления (сообщения) </param>
        /// <param name="message"> Сообщение </param>
        /// <param name="created"> Дата и время создания уведомления </param>
        /// <param name="creator"> Создатель (автор) уведомления </param>
        /// <param name="sent"> Дата и время отправки уведомления </param>
        /// <param name="unsuccessfulAttempts"> Количество сделанных неудачных попыток отправки уведомления</param>
        /// <param name="lastUnsuccessfulAttempt"> Дата и время последней неудачной отправки уведомления </param>
        public Notification(
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
            DateTime? lastUnsuccessfulAttempt = null
            )
        {
            Id = id;
            NotificationMethod = notificationMethod;
            ModelEntityType = modelEntityType;
            BuyerId = buyerId;
            ChangedEntityId = changedEntityId;
            Sender = sender;
            Recipient = recipient;
            Topic = topic;
            Message = message;
            Created = created;
            Creator = creator;
            Sent = sent;
            UnsuccessfulAttempts = unsuccessfulAttempts;
            LastUnsuccessfulAttempt = lastUnsuccessfulAttempt;
        }
    }
}