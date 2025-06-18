namespace Orders.API.Contracts.Requests
{
    /// <summary>
    /// Запрос для смены курьера, доставляющего заказ
    /// </summary>
    public class UpdateCourierIdRequest
    {
        /// <summary> *Уникальный идентификатор заказа в системе </summary>
        public required uint OrderId { get; set; } = default!;

        /// <summary> *Уникальный идентификатор курьера в системе </summary>
        public required uint CourierId { get; set; } = default!;

        /// <summary>
        /// Комментарий
        /// </summary>
        public string? Comment { get; set; }
    }
}
