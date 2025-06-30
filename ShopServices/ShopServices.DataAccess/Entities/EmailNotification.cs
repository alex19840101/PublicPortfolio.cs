using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopServices.DataAccess.Entities
{
    public class EmailNotification
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "bigint")]
        public ulong Id { get; private set; }

        /// <summary> Тип измененной модели (сущности) (для классификации уведомлений) по <see cref="Core.Enums.ModelEntityType"/> </summary>
        [Column(TypeName = "integer")]
        public uint ModelEntityType { get; private set; }

        /// <summary> Уникальный идентификатор покупателя </summary>
        [Column(TypeName = "integer")]
        public uint? BuyerId { get; private set; }

        /// <summary> Id измененной сущности в БД </summary>
        [Column(TypeName = "bigint")]
        public ulong ChangedEntityId { get; private set; }

        public string EmailFrom { get; private set; } = default!;
        public string EmailTo { get; private set; } = default!;
        public string Message { get; private set; } = default!;
        public string Topic { get; private set; } = default!;

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

        public EmailNotification(
            ulong id,
            uint modelEntityType,
            uint? buyerId,
            ulong changedEntityId,
            string emailFrom,
            string emailTo,
            string message,
            DateTime created,
            string creator,
            DateTime? sent = null,
            uint unsuccessfulAttempts = 0,
            DateTime? lastUnsuccessfulAttempt = null
            )
        {
            Id = id;
            ModelEntityType = modelEntityType;
            BuyerId = buyerId;
            ChangedEntityId = changedEntityId;
            EmailFrom = emailFrom;
            EmailTo = emailTo;
            Message = message;
            Created = created;
            Creator = creator;
            Sent = sent;
            UnsuccessfulAttempts = unsuccessfulAttempts;
            LastUnsuccessfulAttempt = lastUnsuccessfulAttempt;
        }
    }
}
