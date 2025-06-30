namespace ShopServices.Core.Enums
{
    public enum NotificationMethod : byte
    {
        None = 0,

        Email = 1,
        TelegramMessage = 2,

        SMS = 160
    }
}