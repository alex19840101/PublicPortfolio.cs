using System;
using ShopServices.Abstractions;

namespace ShopServices.Core.Models.Events
{
    /// <summary> Событие, связанное с возвратом </summary>
    public class RefundEventBase : EventBase
    {
        public long TradeId { get; }
        public uint? OrderId { get; }
        public uint? BuyerId { get; }

        /// <summary> Событие, связанное с возвратом </summary>
        public RefundEventBase(Guid id, long tradeId, DateTime created, string message, string notification, uint? buyerId, uint? orderId) : base(id, created, message, notification)
        {
            TradeId = tradeId;
            BuyerId = buyerId;
            OrderId = orderId;
        }
    }
}
