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
        public ulong Id { get; private set; }
        public string SmsFrom { get; private set; } = default!;
        public string SmsTo { get; private set; } = default!;
        public string Message { get; private set; } = default!;

        /// <summary> Дата и время создания уведомления </summary>
        public DateTime Created { get; private set; }

        /// <summary> Дата и время отправки уведомления </summary>
        public DateTime? Sent { get; set; }
    }
}
