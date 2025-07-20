using System;

namespace Orders.API.Contracts.Requests
{
    /// <summary>
    /// Запрос для обновления планируемого срока поставки заказа менеджером, обслуживающим заказ
    /// </summary>
    public class UpdatePlannedDeliveryTimeRequest
    {
        /// <summary> *Уникальный идентификатор заказа в системе </summary>
        public required uint OrderId { get; set; } = default!;

        /// <summary> *Уникальный идентификатор менеджера в системе </summary>
        public required uint ManagerId { get; set; } = default!;

        /// <summary> Планируемый срок поставки заказа </summary>
        public DateTime PlannedDeliveryTime { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string? Comment { get; set; }
    }
}
