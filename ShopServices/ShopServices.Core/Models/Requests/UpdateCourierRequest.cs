using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServices.Core.Models.Requests
{
    public class UpdateCourierRequest
    {
        public uint Id { get; set; }
        public string DriverLicenseCategory { get; set; } = default!;
        public string Transport { get; set; } = default!;
        public string Areas { get; set; } = default!;
        public string DeliveryTimeSchedule { get; set; } = default!;
    }
}
