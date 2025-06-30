namespace ShopServices.Core.Enums
{
    /// <summary> Тип модели (сущности) (для классификации уведомлений) </summary>
    public enum ModelEntityType : byte
    {
        None = 0,

        Availability = 1,
        Buyer = 2,
        Category = 3,
        Courier = 4,
        Delivery = 5,
        EmailNotification = 6,
        Employee = 7,
        Manager = 8,
        Order = 9,
        OrderPosition = 10,
        PhoneNotification = 11,
        Price = 12,
        Product = 13,
        Shop = 14,
        Trade = 15,
        Warehouse = 16,
        Notification = 17
    }
}
