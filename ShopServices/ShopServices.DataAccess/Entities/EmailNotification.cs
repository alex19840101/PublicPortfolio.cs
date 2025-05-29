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
        public uint Id { get; private set; }
        public string EmailFrom { get; private set; } = default!;
        public string EmailTo { get; private set; } = default!;
        public string Message { get; private set; } = default!;

        /// <summary> Дата и время создания уведомления </summary>
        public DateTime Created { get; private set; }

        /// <summary> Дата и время отправки уведомления </summary>
        public DateTime? Sent { get; set; }
    }
}
