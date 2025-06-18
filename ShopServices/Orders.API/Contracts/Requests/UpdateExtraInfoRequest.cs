namespace Orders.API.Contracts.Requests
{
    /// <summary>
    /// Запрос на изменение дополнительной информации в заказе (со стороны покупателя)
    /// </summary>
    public class UpdateExtraInfoRequest
    {
        /// <summary> *Уникальный идентификатор заказа в системе </summary>
        public uint Id { get; set; }

        /// <summary> *Уникальный идентификатор покупателя в системе </summary>
        public uint BuyerId { get; set; }

        /// <summary> Дополнительная информация </summary>
        public string ExtraInfo { get; set; } = default!;
    }
}
