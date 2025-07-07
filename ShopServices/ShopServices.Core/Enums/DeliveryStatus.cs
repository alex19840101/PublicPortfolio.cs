namespace ShopServices.Core.Enums
{
    /// <summary> Статус доставки (перевозки) заказа(ов) </summary>
    public enum DeliveryStatus : uint
    {
        /// <summary> Создается (добавляется) </summary>
        ToAdd = 0,

        /// <summary> Создана </summary>
        Created = 1,

        /// <summary> Планируется </summary>
        Planning = 201,
        /// <summary> Отложена (задерживается) </summary>
        Delayed = 202,
        /// <summary> Запланирована </summary>
        Scheduled = 203,
        /// <summary> Активна (в процессе) </summary>
        InProgress = 204,

        /// <summary> Авария </summary>
        Accident = 500112,

        /// <summary> Другое (требует внимания) </summary>
        Other = 500500,

        /// <summary> Неуспешная </summary>
        Unsuccessful = 504,
        /// <summary> Частично доставлена </summary>
        PartiallyDelivered = 5050,
        /// <summary> Успешная </summary>
        Delivered = 100100,
        /// <summary> Отменена </summary>
        Canceled = 444444,
        /// <summary> Форс-мажор </summary>
        ForceMajeure = 666666,
        /// <summary> Возврат заказа(ов) </summary>
        Refund = 888888
    }
}