using System;
using ShopServices.Abstractions;

namespace ShopServices.Core.Models.Events
{
    /// <summary> Событие, связанное с работником </summary>
    public class EmployeeEventBase : EventBase
    {
        public uint EmployeeId { get; }

        /// <summary> Событие, связанное с работником </summary>
        public EmployeeEventBase(Guid id, DateTime created, string message, string notification, uint employeeId) : base(id, created, message, notification)
        {
            EmployeeId = employeeId;
        }
    }
}
