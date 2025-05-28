using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopServices.DataAccess.Entities
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get; private set; }

        public uint BuyerId { get; private set; }

        public Buyer Buyer { get; private set; } = default!;

        public uint? DeliveryId { get; private set; }

        public uint? ManagerId { get; set; }
        public uint? CourierId { get; set; }

        public ICollection<Product> Products { get; private set; } = [];

        /// <summary> Полная стоимость заказа </summary>
        public decimal Cost { get; set; }

        /// <summary> Валюта </summary>
        public string Currency { get; set; } = default!;

        /// <summary> Дата и время создания заказа </summary>
        public DateTime Created { get; set; }

        /// <summary> Планируемый срок поставки заказа </summary>
        public DateTime PlannedDeliveryTime { get; set; }

        /// <summary> Дата и время фактической доставки заказа </summary>
        public DateTime? Delivered { get; set; }

        /// <summary> Дата и время фактического получения (выдачи) заказа </summary>
        public DateTime? Received { get; set; }

        /// <summary> Дата и время обновления заказа </summary>
        public DateTime? Updated { get; set; }

        /// <summary> Адрес доставки заказа </summary>
        public string? DeliveryAddress { get; set; }
        /// <summary> Магазин доставки заказа </summary>
        public uint? ShopId { get; set; }

        /// <summary> Дополнительная информация </summary>
        public string ExtraInfo { get; set; } = default!;

        /// <summary> В архиве ли (отменен ли) заказ </summary>
        public bool Archieved { get; set; }
    }
}
