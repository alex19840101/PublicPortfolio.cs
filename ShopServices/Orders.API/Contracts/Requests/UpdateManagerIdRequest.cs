namespace Orders.API.Contracts.Requests
{
    /// <summary>
    /// Запрос для смены менеджера, обслуживающего заказ
    /// </summary>
    public class UpdateManagerIdRequest
    {
        /// <summary> *Уникальный идентификатор заказа в системе </summary>
        public required uint OrderId { get; set; } = default!;

        /// <summary> *Уникальный идентификатор менеджера в системе </summary>
        public required uint ManagerId { get; set; } = default!;

        /// <summary>
        /// Комментарий
        /// </summary>
        public string? Comment { get; set; }
    }
}
