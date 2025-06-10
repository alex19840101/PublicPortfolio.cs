using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopServices.DataAccess.Entities
{
    [Table("Managers")]
    public class Manager : Employee
    {
        public Manager(uint id, string login, string name, string surname, string address, string email, string passwordHash, string? nick, string? phone, string? role, uint? granterId, DateTime createdDt, DateTime? lastUpdateDt) : base(id, login, name, surname, address, email, passwordHash, nick, phone, role, granterId, createdDt, lastUpdateDt)
        {
        }

        public ICollection<Order> Orders { get; set; } = [];
        public ICollection<Delivery> Deliveries { get; set; } = [];
    }
}
