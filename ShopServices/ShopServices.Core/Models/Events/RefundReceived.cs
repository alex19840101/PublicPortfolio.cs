using System;

namespace ShopServices.Core.Models.Events
{
    /// <summary> Событие: возврат платежа </summary>
    public class RefundReceived : RefundEventBase
    {
        /// <summary> Событие: возврат платежа </summary>
        public RefundReceived(Guid id, long tradeId, DateTime created, string message, string notification, uint? buyerId, uint? orderId) :
            base(id, tradeId, created, message, notification, buyerId, orderId)
        {
        }
    }
}
