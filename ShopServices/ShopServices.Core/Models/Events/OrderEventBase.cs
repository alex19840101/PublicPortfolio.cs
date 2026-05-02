using System;
using ShopServices.Abstractions;

namespace ShopServices.Core.Models.Events
{
    /// <summary> Событие, связанное с заказом </summary>
    public class OrderEventBase : EventBase
    {
        public uint OrderId { get; }
        public uint BuyerId { get; }

        /// <summary> Событие, связанное с заказом </summary>
        public OrderEventBase(Guid id, DateTime created, string message, string notification, uint buyerId, uint orderId) : base(id, created, message, notification)
        {
            BuyerId = buyerId;
            OrderId = orderId;
        }
    }
}
