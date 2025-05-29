using System;
using System.Collections.Generic;
using System.Text;
using ShopServices.Core.Auth;

namespace ShopServices.Core.Models
{
    public class Courier
    {
        public Employee Employee { get; set; } = default!;
        public string DriverLicenseCategory { get; set; } = default!;
        public string Transport { get; set; } = default!;
        public string Areas { get; set; } = default!;
        public string DeliveryTimeSchedule { get; set; } = default!;
    }
}
