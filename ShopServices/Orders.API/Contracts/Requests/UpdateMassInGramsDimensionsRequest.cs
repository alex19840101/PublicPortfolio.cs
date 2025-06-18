namespace Orders.API.Contracts.Requests
{
    /// <summary>
    /// Запрос для уточнения массы/габаритов заказа (менеджером/курьером)
    /// </summary>
    public class UpdateMassInGramsDimensionsRequest
    {
        /// <summary> *Уникальный идентификатор заказа в системе </summary>
        public required uint OrderId { get; set; } = default!;


        /// <summary> *Уникальный идентификатор курьера в системе </summary>
        public uint? CourierId { get; set; }

        /// <summary> *Уникальный идентификатор менеджера в системе </summary>
        public uint? ManagerId { get; set; }

        /// <summary> *Уникальный идентификатор доставки в системе </summary>
        public uint? DeliveryId { get; set; }

        /// <summary> Масса, г </summary>
        public uint MassInGrams { get; set; }

        /// <summary> Габариты </summary>
        public string Dimensions { get; set; } = default!;

        /// <summary>
        /// Комментарий
        /// </summary>
        public string? Comment { get; set; }
    }
}
