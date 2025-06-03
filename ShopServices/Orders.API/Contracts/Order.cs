using System;
using System.Collections.Generic;

namespace Orders.API.Contracts
{
    /// <summary> Класс заказа </summary>
    public class Order
    {
        /// <summary> *Уникальный идентификатор заказа в системе </summary>
        public uint Id { get; set; }

        /// <summary> *Уникальный идентификатор покупателя в системе </summary>
        public uint BuyerId { get; set; }

        /// <summary> Список товарных позиций в заказе </summary>
        public List<ProductData> Positions { get; set; }

        /// <summary> Полная стоимость заказа </summary>
        public decimal Cost { get; set; }

        /// <summary> Валюта </summary>
        public string Currency { get; set; }

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

        /// <summary> Id курьера, доставляющего заказ </summary>
        public uint? CourierId { get; set; }

        /// <summary> Id доставки заказа </summary>
        public uint? DeliveryId { get; set; }

        /// <summary> Адрес доставки заказа </summary>
        public string DeliveryAddress { get; set; }

        /// <summary> Id менеджера </summary>
        public uint? ManagerId { get; set; }

        /// <summary> Дополнительная информация </summary>
        public string ExtraInfo { get; set; }

        /// <summary> В архиве ли (отменен ли) заказ </summary>
        public bool Archieved { get; set; }

        /// <summary> Масса, г </summary>
        public uint MassInGrams { get; set; }

        /// <summary> Габариты </summary>
        public string Dimensions { get; set; }
    }
}
