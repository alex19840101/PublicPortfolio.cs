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

        /// <summary> Навигационное свойство: покупатель </summary>
        public Buyer Buyer { get; private set; }

        /// <summary> Навигационное свойство: список товарных позиций в заказе </summary>
        public ICollection<OrderPosition> Positions { get; } = [];

        /// <summary> Навигационное свойство: доставка </summary>
        public Delivery? Delivery { get; private set; }
        /// <summary> Навигационное свойство: коллекция перевозок </summary>
        public ICollection<Delivery>? Deliveries { get; }
        public uint? DeliveryId { get; private set; }

        public uint? ManagerId { get; private set; }
        public uint? CourierId { get; private set; }

        /// <summary> Навигационное свойство: коллекция товаров из заказа </summary>
        public ICollection<Product> Products { get; } = [];

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

        /// <summary> Навигационное свойство: магазин доставки заказа </summary>
        public Shop? Shop { get; private set; }

        /// <summary> Магазин доставки заказа </summary>
        public uint? ShopId { get; private set; }

        /// <summary> Дополнительная информация </summary>
        public string ExtraInfo { get; private set; } = default!;

        /// <summary> В архиве ли (отменен ли) заказ </summary>
        public bool Archieved { get; private set; }

        /// <summary> Масса, г </summary>
        [Column(TypeName = "integer")]
        public uint MassInGrams { get; private set; }

        /// <summary> Габариты </summary>
        public string Dimensions { get; private set; } = default!;

        /// <summary> Комментарий </summary>
        public string? Comment { get; private set; }

        public Order(
            uint id,
            uint buyerId,
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
            DateTime? updated = null,
            string? comment = null)
        {
            Id = id;
            BuyerId = buyerId;
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
            Comment = comment;
        }

        /// <summary> Отмена заказа (Archieved = true) </summary>
        internal void Cancel() => Archieved = true;
        internal void UpdateComment(string? comment) => Comment = comment;
        internal void UpdateCourierId(uint? courierId) => CourierId = courierId;
        internal void UpdateDelivered(DateTime delivered) => Delivered = delivered;
        internal void UpdateDeliveryAddress(string? deliveryAddress) => DeliveryAddress = deliveryAddress;
        internal void UpdateDeliveryId(uint? deliveryId) => DeliveryId = deliveryId;
        internal void UpdateDimensions(string dimensions) => Dimensions = dimensions;
        internal void UpdateExtraInfo(string extraInfo) => ExtraInfo = extraInfo;
        internal void UpdateManagerId(uint? managerId) => ManagerId = managerId;
        internal void UpdateMassInGrams(uint massInGrams) => MassInGrams = massInGrams;
        internal void UpdatePaymentInfo(string paymentInfo) => PaymentInfo = paymentInfo;
        internal void UpdatePlannedDeliveryTime(DateTime plannedDeliveryTime) => PlannedDeliveryTime = plannedDeliveryTime;
        internal void UpdateReceived(DateTime received) => Received = received;
        internal void UpdateUpdatedDt(DateTime updatedDt) => Updated = updatedDt;
        internal void UpdateShopId(uint? shopId) => ShopId = shopId;
        internal void SetCost(decimal cost) => Cost = cost;
    }
}
