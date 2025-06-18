namespace Orders.API.Contracts.Requests
{
    /// <summary>
    /// Запрос на отмену заказа (со стороны покупателя)
    /// </summary>
    public class CancelOrderRequest
    {
        /// <summary> *Уникальный идентификатор заказа в системе </summary>
        public uint Id { get; set; }

        /// <summary> *Уникальный идентификатор покупателя в системе </summary>
        public uint BuyerId { get; set; }

        /// <summary>
        /// Строка подтверждения отмены заказа
        /// </summary>
        public string ConfirmationString { get; set; } = default!;
    }
}
