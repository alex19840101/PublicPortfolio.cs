namespace Orders.API.Contracts.Requests
{
    /// <summary>
    /// Запрос для обновления информации об оплате (со стороны менеджера/курьера, обслуживающего заказ)
    /// </summary>
    public class UpdatePaymentInfoByManagerRequest
    {
        /// <summary> *Уникальный идентификатор заказа в системе </summary>
        public required uint OrderId { get; set; } = default!;

        /// <summary> Уникальный идентификатор менеджера в системе </summary>
        public uint? ManagerId { get; set; } = default!;

        /// <summary> Уникальный идентификатор курьера в системе </summary>
        public uint? CourierId { get; set; }

        /// <summary> Информация по оплате </summary>
        public string PaymentInfo { get; set; } = default!;

        /// <summary>
        /// Комментарий
        /// </summary>
        public string? Comment { get; set; }


    }
}
