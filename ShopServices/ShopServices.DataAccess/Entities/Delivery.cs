using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopServices.DataAccess.Entities
{
    public class Delivery
    {
        /// <summary> Уникальный идентификатор доставки заказа </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get; private set; }

        /// <summary> Уникальный идентификатор покупателя </summary>
        public uint BuyerId { get; private set; }
        
        /// <summary> Уникальный идентификатор заказа </summary>
        public uint OrderId { get; private set; }

        public Buyer Buyer { get; private set; } = default!;

        /// <summary> Адрес доставки заказа </summary>
        public required string Address { get; set; } = default!;
        
        /// <summary> Уникальный идентификатор менеджера </summary>
        public uint? ManagerId { get; set; }
        
        /// <summary> Уникальный идентификатор курьера </summary>
        public uint? CourierId { get; set; }

        /// <summary> Информация по оплате </summary>
        public string PaymentInfo { get; set; } = default!;

        /// <summary> Масса, г </summary>
        public uint MassInGrams { get; set; } = default!;

        /// <summary> Габариты </summary>
        public string Dimensions { get; set; } = default!;

        /// <summary> Дата и время создания заказа на доставку </summary>
        public DateTime Created { get; set; }

        /// <summary> Дата и время обновления данных заказа на доставку </summary>
        public DateTime? Updated { get; set; }
    }
}
