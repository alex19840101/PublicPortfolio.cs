using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopServices.DataAccess.Entities
{
    public class Courier
    {
        public Employee Employee { get; set; } = default!;
    }
}
