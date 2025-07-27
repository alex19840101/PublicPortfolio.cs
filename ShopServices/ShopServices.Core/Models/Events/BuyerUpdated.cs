using System;

namespace ShopServices.Core.Models.Events
{
    /// <summary> Событие: покупатель обновил личные данные </summary>
    public class BuyerUpdated : BuyerEventBase
    {
        /// <summary> Событие: покупатель обновил личные данные </summary>
        public BuyerUpdated(Guid id, DateTime created, string message, string notification, uint buyerId) : base(id, created, message, notification, buyerId)
        {
        }
    }
}
