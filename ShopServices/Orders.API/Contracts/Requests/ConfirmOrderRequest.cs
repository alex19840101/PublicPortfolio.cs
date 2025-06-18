namespace Orders.API.Contracts.Requests
{
    /// <summary>
    /// Запрос на подтверждение заказа (со стороны покупателя)
    /// </summary>
    public class ConfirmOrderRequest
    {
        /// <summary> *Уникальный идентификатор заказа в системе </summary>
        public uint Id { get; set; }

        /// <summary> *Уникальный идентификатор покупателя в системе </summary>
        public uint BuyerId { get; set; }

        /// <summary>
        /// Строка подтверждения заказа
        /// </summary>
        public string ConfirmationString { get; set; } = default!;
    }
}
