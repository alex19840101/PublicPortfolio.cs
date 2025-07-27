namespace ShopServices.Core
{
    public class NotificationMessages
    {
        public const string NOTIFICATION_API = "Notification.API";
        public const string EMAIL_FROM = "{from}@{host}.{zone}";
        public const string SMS_FROM = "{phone-sender}";
        public const string TELEGRAM_NOTIFICATION_FROM = "ShopServices";
        public const string ORDER_CREATED = "Заказ {id} создан";
        public const string ORDER_CANCELED = "Заказ {id} отменен";
        public const string ORDER_DELIVERED_TO_BUYER = "Заказ {id} доставлен покупателю";
        public const string ORDER_DELIVERED_TO_SHOP = "Заказ {id} доставлен в магазин";

        public const string BUYER = "Buyer";
        public const string SHOP = "Shop";
    }
}
