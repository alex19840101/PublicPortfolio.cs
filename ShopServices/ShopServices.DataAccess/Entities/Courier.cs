using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopServices.DataAccess.Entities
{
    [Table("Couriers")]
    public class Courier : Employee
    {
        public Courier(uint id, string login, string name, string surname, string address, string email, string passwordHash, string? nick, string? phone, string? role, uint? granterId, DateTime createdDt, DateTime? lastUpdateDt) : base(id, login, name, surname, address, email, passwordHash, nick, phone, role, granterId, createdDt, lastUpdateDt)
        {
        }

        public ICollection<Order> Orders { get; set; } = [];
        public ICollection<Delivery> Deliveries { get; set; } = [];
        public string? DriverLicenseCategory { get; set; }
        public string Transport { get; set; } = default!;
        public string Areas { get; set; } = default!;
        public string DeliveryTimeSchedule { get; set; } = default!;
    }
}
