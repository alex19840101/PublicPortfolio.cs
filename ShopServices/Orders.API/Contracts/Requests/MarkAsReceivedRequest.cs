namespace Orders.API.Contracts.Requests
{
    /// <summary>
    /// Запрос для отметки заказа как полученного покупателем
    /// </summary>
    public class MarkAsReceivedRequest
    {
        /// <summary> *Уникальный идентификатор заказа в системе </summary>
        public required uint OrderId { get; set; } = default!;

        /// <summary> Комментарий </summary>
        public string? Comment { get; set; }

        /// <summary> Уникальный идентификатор курьера в системе </summary>
        public uint? CourierId { get; set; }

        /// <summary> Уникальный идентификатор менеджера в системе </summary>
        public uint? ManagerId { get; set; }
    }
}
