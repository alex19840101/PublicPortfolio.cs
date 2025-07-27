using System;

namespace ShopServices.Core.Models.Events
{
    /// <summary> Событие: работник изменил личные данные </summary>
    public class EmployeeUpdated : EmployeeEventBase
    {
        /// <summary> Событие: работник изменил личные данные </summary>
        public EmployeeUpdated(Guid id, DateTime created, string message, string notification, uint employeeId) : base(id, created, message, notification, employeeId)
        {
        }
    }
}
