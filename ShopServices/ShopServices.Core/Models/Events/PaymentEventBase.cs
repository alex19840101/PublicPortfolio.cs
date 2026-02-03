using System;
using ShopServices.Abstractions;

namespace ShopServices.Core.Models.Events
{
    /// <summary> Событие, связанное с платежом </summary>
    public class PaymentEventBase : EventBase
    {
        public long TradeId { get; }
        public uint? OrderId { get; }
        public uint? BuyerId { get; }

        /// <summary> Событие, связанное с платежом </summary>
        public PaymentEventBase(Guid id, long tradeId, DateTime created, string message, string notification, uint? buyerId, uint? orderId) :
            base(id, created, message, notification)
        {
            TradeId = tradeId;
            BuyerId = buyerId;
            OrderId = orderId;
        }
    }
}
