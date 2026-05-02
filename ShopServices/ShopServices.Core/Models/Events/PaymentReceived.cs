using System;

namespace ShopServices.Core.Models.Events
{
    /// <summary> Событие: поступила оплата </summary>
    public class PaymentReceived : PaymentEventBase
    {
        /// <summary> Событие: поступила оплата </summary>
        public PaymentReceived(Guid id, long tradeId, DateTime created, string message, string notification, uint? buyerId, uint? orderId) :
            base(id, tradeId, created, message, notification, buyerId, orderId)
        {
        }

        public override string ToString()
        {
            return $"Payment(tradeId={TradeId},buyer={BuyerId},order={OrderId})";
        }
    }
}
