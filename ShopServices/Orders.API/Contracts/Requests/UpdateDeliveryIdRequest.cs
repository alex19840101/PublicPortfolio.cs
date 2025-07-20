namespace Orders.API.Contracts.Requests
{
    /// <summary>
    /// Запрос для смены идентификатора доставки (менеджером/курьером) (в случаях изменений адреса/магазина доставки/отмены заказа...)
    /// </summary>
    public class UpdateDeliveryIdRequest
    {
        /// <summary> *Уникальный идентификатор заказа в системе </summary>
        public required uint OrderId { get; set; } = default!;


        /// <summary> Уникальный идентификатор курьера в системе </summary>
        public uint? CourierId { get; set; }

        /// <summary> Уникальный идентификатор менеджера в системе </summary>
        public uint? ManagerId { get; set; }

        /// <summary> *Уникальный идентификатор доставки в системе </summary>
        public uint? DeliveryId { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string? Comment { get; set; }
    }
}
