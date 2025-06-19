namespace Orders.API.Contracts.Requests
{
    /// <summary>
    /// Запрос для отметки заказа как полученного покупателем
    /// </summary>
    public class MarkAsReceivedRequest
    {
        /// <summary> *Уникальный идентификатор заказа в системе </summary>
        public required uint OrderId { get; set; } = default!;

        /// <summary>
        /// Комментарий
        /// </summary>
        public string? Comment { get; set; }
    }
}
