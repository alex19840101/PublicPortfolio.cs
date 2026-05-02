using System;

namespace ShopServices.Core.Models.Events
{
    /// <summary> Событие: заказ отменен </summary>
    public class OrderCanceled : OrderEventBase
    {
        /// <summary> Событие: заказ отменен </summary>
        public OrderCanceled(Guid id, DateTime created, string message, string notification, uint buyerId, uint orderId) : base(id, created, message, notification, buyerId, orderId)
        {
        }
    }
}
