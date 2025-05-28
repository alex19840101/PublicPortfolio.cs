using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopServices.DataAccess.Entities
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get; private set; }

        public uint BuyerId { get; private set; }

        public Buyer Buyer { get; private set; } = default!;

        public uint? DeliveryId { get; private set; }

        public uint? ManagerId { get; set; }
        public uint? CourierId { get; set; }
    }
}
