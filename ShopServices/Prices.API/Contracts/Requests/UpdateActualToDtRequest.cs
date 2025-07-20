using System;

namespace Prices.API.Contracts.Requests
{
    /// <summary> Запрос на обновление ActualToDt ценника на товар </summary>
    public class UpdateActualToDtRequest
    {
        /// <summary> Id ценника на товара* </summary>
        public uint PriceId { get; set; }
        
        /// <summary> Актуально до какого времени </summary>
        public DateTime? ActualToDt { get; set; }
    }
}
