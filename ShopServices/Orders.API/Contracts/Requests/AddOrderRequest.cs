using System;
using System.Collections.Generic;

namespace Orders.API.Contracts.Requests
{
    /// <summary> Запрос на добавление заказа </summary>
    public class AddOrderRequest
    {
        /// <summary> *Уникальный идентификатор покупателя в системе </summary>
        public uint BuyerId { get; set; }

        /// <summary> Список товарных позиций в заказе </summary>
        public List<AddOrderPositionRequest> Positions { get; set; } = default!;

        /// <summary> Полная стоимость заказа </summary>
        public decimal Cost { get; set; }

        /// <summary> Валюта </summary>
        public string Currency { get; set; } = default!;

        /// <summary> Дата и время создания заказа </summary>
        public DateTime Created { get; set; }

        /// <summary> Планируемый срок поставки заказа </summary>
        public DateTime PlannedDeliveryTime { get; set; }

        /// <summary> Информация по оплате </summary>
        public string PaymentInfo { get; set; } = default!;

        /// <summary> Id доставки заказа </summary>
        public uint? DeliveryId { get; set; }

        /// <summary> Адрес доставки заказа (в случае доставки не в магазин) </summary>
        public string? DeliveryAddress { get; set; }

        /// <summary> Магазин доставки заказа </summary>
        public uint? ShopId { get; set; }

        /// <summary> Дополнительная информация </summary>
        public string ExtraInfo { get; set; } = default!;

        /// <summary> Масса, г (предполагается вычисление и заполнение веб-клиентом на основе набора позиций заказа) </summary>
        public uint? MassInGrams { get; set; }

        /// <summary> Габариты (предполагается вычисление и заполнение веб-клиентом на основе набора позиций заказа) </summary>
        public string? Dimensions { get; set; }
    }
}
