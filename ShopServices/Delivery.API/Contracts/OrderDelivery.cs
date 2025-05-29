using System;

namespace Delivery.API.Contracts
{
    /// <summary> Данные доставки заказа </summary>
    public class OrderDelivery
    {
        /// <summary> Уникальный идентификатор доставки заказа </summary>
        public uint Id { get; set; }
        
        /// <summary> Уникальный идентификатор покупателя </summary>
        public uint BuyerId { get; set; }

        /// <summary> Уникальный идентификатор заказа </summary>
        public uint OrderId { get; set; }

        /// <summary> Информация по оплате </summary>
        public string PaymentInfo { get; set; }

        /// <summary> Уникальный идентификатор курьера </summary>
        public uint? CourierId { get; set; }

        /// <summary> Адрес доставки заказа </summary>
        public string Address { get; set; }

        /// <summary> Масса, г </summary>
        public string MassInGrams { get; set; }

        /// <summary> Габариты </summary>
        public string Dimensions { get; set; }

        /// <summary> Дата и время создания заказа на доставку </summary>
        public DateTime Created { get; set; }

        /// <summary> Дата и время обновления данных заказа на доставку </summary>
        public DateTime? Updated { get; set; }
    }
}
