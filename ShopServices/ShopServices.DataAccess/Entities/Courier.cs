using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopServices.DataAccess.Entities
{
    public class Courier
    {
        [Required]
        public Employee Employee { get; set; } = default!;
        public ICollection<Order> Orders { get; set; } = [];
        public ICollection<Delivery> Deliveries { get; set; } = [];
        public string? DriverLicenseCategory { get; set; }
        public string Transport { get; set; } = default!;
        public string Areas { get; set; } = default!;
        public string DeliveryTimeSchedule { get; set; } = default!;
    }
}
