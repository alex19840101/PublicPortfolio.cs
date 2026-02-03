namespace ShopServices.Core.Models.Events
{
    public class EventsTopics
    {
        /// <summary> топик для простых строковых сообщений </summary>
        public const string LEGACY_PAYMENT_RECIEVED = "NewPayment";
        /// <summary> топик для простых строковых сообщений </summary>
        public const string LEGACY_REFUND_RECIEVED = "NewRefund";

        public const string PAYMENT_RECIEVED = "Payment";
        public const string REFUND_RECIEVED = "Refund";
    }
}
