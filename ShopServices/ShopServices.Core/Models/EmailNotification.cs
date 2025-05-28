using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServices.Core.Models
{
    public class EmailNotification
    {
        public uint Id { get; private set; }
        public string EmailFrom { get; private set; }
        public string EmailTo { get; private set; }
        public string Message { get; private set; }

        /// <summary> Дата и время создания уведомления </summary>
        public DateTime Created { get; private set; }

        /// <summary> Дата и время отправки уведомления </summary>
        public DateTime? Sent { get; set; }
    }
}
