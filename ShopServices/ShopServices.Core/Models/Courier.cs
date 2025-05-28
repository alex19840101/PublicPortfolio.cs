using System;
using System.Collections.Generic;
using System.Text;
using ShopServices.Core.Auth;

namespace ShopServices.Core.Models
{
    public class Courier
    {
        public Employee Employee { get; set; } = default!;
    }
}
