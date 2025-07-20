using System;

namespace Prices.API.Contracts.Responses
{
    /// <summary>
    /// Класс данных ценника на товар
    /// </summary>
    public class PriceResponseDto
    {
        /// <summary> Id ценника (не указывается в запросе на добавление ценника) </summary>
        public uint Id { get; set; }

        /// <summary> Id товара* </summary>
        public uint ProductId { get; set; }

        /// <summary> Цена за единицу измерения* </summary>
        public decimal PricePerUnit { get; set; }

        /// <summary> Валюта* </summary>
        public string Currency { get; set; } = default!;

        /// <summary> Единица измерения товара* </summary>
        public string Unit { get; set; } = default!;

        /// <summary> Актуально от какого времени </summary>
        public DateTime ActualFromDt { get; set; }

        /// <summary> Актуально до какого времени </summary>
        public DateTime? ActualToDt { get; set; }

        /// <summary> Дата и время обновления ActualToDt (опционально) </summary>
        public DateTime? Updated { get; set; }
    }
}
