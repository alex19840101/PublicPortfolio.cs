using System;
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

        /// <summary> Список товарных позиций в заказе </summary>
        public ICollection<OrderPosition> Positions { get; private set; } = [];

        public uint? DeliveryId { get; private set; }

        public uint? ManagerId { get; private set; }
        public uint? CourierId { get; private set; }

        public ICollection<Product> Products { get; private set; } = [];

        /// <summary> Полная стоимость заказа </summary>
        public decimal Cost { get; private set; }

        /// <summary> Валюта </summary>
        public string Currency { get; private set; } = default!;

        /// <summary> Дата и время создания заказа </summary>
        public DateTime Created { get; private set; }

        /// <summary> Планируемый срок поставки заказа </summary>
        public DateTime PlannedDeliveryTime { get; private set; }

        /// <summary> Дата и время фактической доставки заказа </summary>
        public DateTime? Delivered { get; private set; }

        /// <summary> Информация по оплате </summary>
        public string PaymentInfo { get; private set; } = default!;

        /// <summary> Дата и время фактического получения (выдачи) заказа </summary>
        public DateTime? Received { get; private set; }

        /// <summary> Дата и время обновления заказа </summary>
        public DateTime? Updated { get; private set; }

        /// <summary> Адрес доставки заказа (в случае доставки не в магазин) </summary>
        public string? DeliveryAddress { get; private set; }
        /// <summary> Магазин доставки заказа </summary>
        public uint? ShopId { get; private set; }

        /// <summary> Дополнительная информация </summary>
        public string ExtraInfo { get; private set; } = default!;

        /// <summary> В архиве ли (отменен ли) заказ </summary>
        public bool Archieved { get; private set; }

        /// <summary> Масса, г </summary>
        public uint MassInGrams { get; private set; } = default!;

        /// <summary> Габариты </summary>
        public string Dimensions { get; private set; } = default!;

        public Order(
            uint id,
            uint buyerId,
            Buyer buyer,
            ICollection<OrderPosition> positions,
            decimal cost,
            string currency,
            DateTime created,
            DateTime plannedDeliveryTime,
            string paymentInfo,
            string? deliveryAddress,
            uint? shopId,
            string extraInfo,
            bool archieved,
            uint massInGrams,
            string dimensions,
            uint? deliveryId = null,
            uint? managerId = null,
            uint? courierId = null,
            DateTime? received = null,
            DateTime? delivered = null,
            DateTime? updated = null)
        {
            Id = id;
            BuyerId = buyerId;
            Buyer = buyer;
            Positions = positions;
            Cost = cost;
            Currency = currency;
            Created = created;
            PlannedDeliveryTime = plannedDeliveryTime;
            PaymentInfo = paymentInfo;
            DeliveryAddress = deliveryAddress;
            ShopId = shopId;
            ExtraInfo = extraInfo;
            Archieved = archieved;
            MassInGrams = massInGrams;
            Dimensions = dimensions;
            DeliveryId = deliveryId;
            ManagerId = managerId;
            CourierId = courierId;
            Received = received;
            Delivered = delivered;
            Updated = updated;
        }
    }
}
