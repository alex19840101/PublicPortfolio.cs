namespace ShopServices.Core
{
    public class NotificationMessages
    {
        public const string NOTIFICATION_API = "Notification.API";
        public const string EMAIL_FROM = "{from}@{host}.{zone}";
        public const string SMS_FROM = "{phone-sender}";
        public const string TELEGRAM_NOTIFICATION_FROM = "ShopServices";
        public const string ORDER_CREATED = "Заказ создан";
        public const string ORDER_CANCELED = "Заказ отменен";
        public const string ORDER_DELIVERED = "Заказ доставлен";
    }
}
