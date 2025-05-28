using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopServices.DataAccess.Entities
{
    public class Manager
    {
        [Required]
        public Employee Employee { get; set; } = default!;
        public ICollection<Order> Orders { get; set; } = [];
        public ICollection<Delivery> Deliveries { get; set; } = [];
    }
}
