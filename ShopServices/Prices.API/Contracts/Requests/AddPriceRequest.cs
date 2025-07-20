using System;

namespace Prices.API.Contracts.Requests
{
    /// <summary>
    /// Запрос с данными на добавление ценника на товар
    /// </summary>
    public class AddPriceRequest
    {
        /// <summary> Id товара* </summary>
        public uint ProductId { get; set; }

        /// <summary> Цена за единицу измерения </summary>
        public decimal PricePerUnit { get; set; }

        /// <summary> Валюта </summary>
        public string Currency { get; set; } = default!;

        /// <summary> Единица измерения товара </summary>
        public string Unit { get; set; } = default!;

        /// <summary> Актуально от какого времени </summary>
        public DateTime ActualFromDt { get; set; }

        /// <summary> Актуально до какого времени </summary>
        public DateTime? ActualToDt { get; set; }
    }
}
