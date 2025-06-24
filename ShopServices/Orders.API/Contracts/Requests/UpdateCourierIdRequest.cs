namespace Orders.API.Contracts.Requests
{
    /// <summary>
    /// Запрос для смены/сброса курьера, доставляющего заказ
    /// </summary>
    public class UpdateCourierIdRequest
    {
        /// <summary> *Уникальный идентификатор заказа в системе </summary>
        public required uint OrderId { get; set; } = default!;

        /// <summary> Уникальный идентификатор курьера в системе </summary>
        public uint? CourierId { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string? Comment { get; set; }
    }
}
