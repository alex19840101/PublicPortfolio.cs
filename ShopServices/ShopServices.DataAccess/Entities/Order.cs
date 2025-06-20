﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopServices.DataAccess.Entities
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "integer")]
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

        /// <summary> Информация по оплате </summary>
        public string PaymentInfo { get; set; } = default!;

        /// <summary> Дата и время фактического получения (выдачи) заказа </summary>
        public DateTime? Received { get; set; }

        /// <summary> Дата и время обновления заказа </summary>
        public DateTime? Updated { get; set; }

        /// <summary> Адрес доставки заказа (в случае доставки не в магазин) </summary>
        public string? DeliveryAddress { get; set; }
        /// <summary> Магазин доставки заказа </summary>
        public uint? ShopId { get; set; }

        /// <summary> Дополнительная информация </summary>
        public string ExtraInfo { get; set; } = default!;

        /// <summary> В архиве ли (отменен ли) заказ </summary>
        public bool Archieved { get; set; }

        /// <summary> Масса, г </summary>
        public uint MassInGrams { get; set; } = default!;

        /// <summary> Габариты </summary>
        public string Dimensions { get; set; } = default!;
    }
}
