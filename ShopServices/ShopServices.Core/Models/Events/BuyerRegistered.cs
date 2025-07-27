using System;

namespace ShopServices.Core.Models.Events
{
    /// <summary> Событие: покупатель зарегистрировался </summary>
    public class BuyerRegistered : BuyerEventBase
    {
        /// <summary> Событие: покупатель зарегистрировался </summary>
        public BuyerRegistered(Guid id, DateTime created, string message, string notification, uint buyerId) : base(id, created, message, notification, buyerId)
        {
        }
    }
}
