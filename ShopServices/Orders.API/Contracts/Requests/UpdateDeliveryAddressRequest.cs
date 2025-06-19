namespace Orders.API.Contracts.Requests
{
    /// <summary>
    /// Запрос на изменение адреса доставки заказа (со стороны покупателя)
    /// </summary>
    public class UpdateDeliveryAddressRequest
    {
        /// <summary> *Уникальный идентификатор заказа в системе </summary>
        public uint OrderId { get; set; }

        /// <summary> *Уникальный идентификатор покупателя в системе </summary>
        public uint BuyerId { get; set; }

        /// <summary> Адрес доставки заказа (в случае доставки не в магазин) </summary>
        public string? DeliveryAddress { get; set; }
    }
}
