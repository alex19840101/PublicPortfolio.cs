namespace Orders.API.Contracts.Requests
{
    /// <summary>
    /// Запрос на изменение магазина выдачи заказа (со стороны покупателя)
    /// </summary>
    public class UpdateShopIdRequest
    {
        /// <summary> *Уникальный идентификатор заказа в системе </summary>
        public uint Id { get; set; }

        /// <summary> *Уникальный идентификатор покупателя в системе </summary>
        public uint BuyerId { get; set; }

        /// <summary> Магазин доставки заказа </summary>
        public uint? ShopId { get; set; }
    }
}
