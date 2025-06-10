using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServices.Core.Models
{
    public class PhoneNotification
    {
        public ulong Id { get; private set; }

        public string SmsFrom { get; private set; }
        public string SmsTo { get; private set; }
        public string Message { get; private set; }

        /// <summary> Дата и время создания уведомления </summary>
        public DateTime Created { get; private set; }

        /// <summary> Дата и время отправки уведомления </summary>
        public DateTime? Sent { get; set; }
    }
}
