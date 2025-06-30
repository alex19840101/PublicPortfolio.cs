using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopServices.DataAccess.Entities
{
    public class PhoneNotification
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "bigint")]
        public ulong Id { get; private set; }

        /// <summary> *Метод уведомления по <see cref="ShopServices.Core.Enums.NotificationMethod"/> </summary>
        [Column(TypeName = "integer")]
        public uint NotificationMethod { get; private set; }

        /// <summary> Тип измененной модели (сущности) (для классификации уведомлений) по <see cref="Core.Enums.ModelEntityType"/> </summary>
        [Column(TypeName = "integer")]
        public uint ModelEntityType { get; private set; }

        /// <summary> Уникальный идентификатор покупателя </summary>
        [Column(TypeName = "integer")]
        public uint? BuyerId { get; private set; }

        /// <summary> Id измененной сущности в БД </summary>
        [Column(TypeName = "bigint")]
        public ulong ChangedEntityId { get; private set; }
        public string SmsFrom { get; private set; } = default!;
        public string SmsTo { get; private set; } = default!;
        public string Message { get; private set; } = default!;

        /// <summary> Дата и время создания уведомления </summary>
        public DateTime Created { get; private set; }

        /// <summary> Создатель (автор) уведомления </summary>
        public string Creator { get; private set; } = default!;

        /// <summary> Дата и время отправки уведомления </summary>
        public DateTime? Sent { get; private set; }

        [Column(TypeName = "integer")]
        public uint UnsuccessfulAttempts { get; private set; } = 0;

        /// <summary> Дата и время последней неудачной отправки уведомления </summary>
        public DateTime? LastUnsuccessfulAttempt { get; private set; }

        /// <summary>
        /// ShopServices.DataAccess.Entities.PhoneNotification - конструктор уведомления
        /// </summary>
        /// <param name="id"> Id однократного уведомления в таблице телефонных уведомлений PhoneNotifications </param>
        /// <param name="notificationMethod"> *Метод уведомления по <see cref="ShopServices.Core.Enums.NotificationMethod"/> </param>
        /// <param name="modelEntityType"> Тип измененной модели (сущности) (для классификации уведомлений) по <see cref="Enums.ModelEntityType"/> </param>
        /// <param name="buyerId"> Уникальный идентификатор покупателя </param>
        /// <param name="changedEntityId"> Id измененной сущности в БД </param>
        /// <param name="smsFrom"> Телефонный номер отправителя уведомления (сообщения) </param>
        /// <param name="smsTo"> Телефонный номер получателя уведомления (сообщения) </param>
        /// <param name="message"> Сообщение </param>
        /// <param name="created"> Дата и время создания уведомления </param>
        /// <param name="creator"> Создатель (автор) уведомления </param>
        /// <param name="sent"> Дата и время отправки уведомления </param>
        /// <param name="unsuccessfulAttempts"> Количество сделанных неудачных попыток отправки уведомления</param>
        /// <param name="lastUnsuccessfulAttempt"> Дата и время последней неудачной отправки уведомления </param>
        public PhoneNotification(
            ulong id,
            uint notificationMethod,
            uint modelEntityType,
            uint? buyerId,
            ulong changedEntityId,
            string smsFrom,
            string smsTo,
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
            SmsFrom = smsFrom;
            SmsTo = smsTo;
            Message = message;
            Created = created;
            Creator = creator;
            Sent = sent;
            UnsuccessfulAttempts = unsuccessfulAttempts;
            LastUnsuccessfulAttempt = lastUnsuccessfulAttempt;
        }
    }
}
