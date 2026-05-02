using System;

namespace ShopServices.Core.Models.Events
{
    /// <summary> Событие: заказ создан </summary>
    public class OrderCreated : OrderEventBase
    {
        /// <summary> Событие: заказ создан </summary>
        public OrderCreated(Guid id, DateTime created, string message, string notification, uint buyerId, uint orderId) : base(id, created, message, notification, buyerId, orderId)
        {
        }
    }
}
