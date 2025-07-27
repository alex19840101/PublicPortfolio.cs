using System;

namespace ShopServices.Core.Models.Events
{
    /// <summary> Событие: работник зарегистрировался </summary>
    public class EmployeeRegistered : EmployeeEventBase
    {
        /// <summary> Событие: работник зарегистрировался </summary>
        public EmployeeRegistered(Guid id, DateTime created, string message, string notification, uint employeeId) : base(id, created, message, notification, employeeId)
        {
        }
    }
}
