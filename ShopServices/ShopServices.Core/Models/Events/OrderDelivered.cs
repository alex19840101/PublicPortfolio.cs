using System;

namespace ShopServices.Core.Models.Events
{
    /// <summary> Событие: заказ доставлен в пункт выдачи </summary>
    public class OrderDelivered : OrderEventBase
    {
        /// <summary> Событие: заказ доставлен в пункт выдачи </summary>
        public OrderDelivered(Guid id, DateTime created, string message, string notification, uint buyerId, uint orderId) : base(id, created, message, notification, buyerId, orderId)
        {
        }
    }
}
