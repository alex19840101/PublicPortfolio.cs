using System;
using System.Collections.Generic;

namespace Trade.API.Contracts.Requests
{
    /// <summary> Запрос на добавление оплаты </summary>
    public class AddPaymentRequest
    {
        /// <summary> Id заказа (при заказе) </summary>
        public uint? OrderId { get; set; }

        /// <summary> Id покупателя (при заказе) </summary> 
        public uint? BuyerId { get; set; }

        /// <summary> Список товарных позиций в заказе/чеке/квитанции </summary>
        public List<OrderPositionRequest> Positions { get; set; } = default!;

        /// <summary> Полная стоимость заказа/чека/квитанции </summary>
        public decimal Cost { get; set; }

        /// <summary> Валюта </summary>
        public string Currency { get; set; } = default!;

        /// <summary> Дата и время оплаты </summary>
        public DateTime Created { get; set; }

        /// <summary> Информация по оплате </summary>
        public string PaymentInfo { get; set; } = default!;

        /// <summary> Дополнительная информация </summary>
        public string? ExtraInfo { get; set; }

        /// <summary> Комментарий </summary>
        public string? Comment { get; set; }

        /// <summary> Магазин покупки </summary>
        public uint? ShopId { get; set; }
        
        /// <summary> Id менеджера (при оплате менеджеру) </summary>
        public uint? ManagerId { get; set; }

        /// <summary> Id курьера (при оплате курьеру) </summary>
        public uint? CourierId { get; set; }
    }
}
