using System;
using ShopServices.Abstractions;

namespace ShopServices.Core.Models.Events
{
    /// <summary> Событие, связанное с покупателем </summary>
    public class BuyerEventBase : EventBase
    {
        public uint BuyerId { get; }

        /// <summary> Событие, связанное с покупателем </summary>
        public BuyerEventBase(Guid id, DateTime created, string message, string notification, uint buyerId) : base(id, created, message, notification)
        {
            BuyerId = buyerId;
        }
    }
}
