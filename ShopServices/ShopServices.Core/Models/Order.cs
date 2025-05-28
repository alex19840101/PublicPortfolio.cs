namespace ShopServices.Core.Models
{
    internal class Order
    {
        public uint Id { get; private set; }

        public uint BuyerId { get; private set; }

        public Buyer Buyer { get; private set; } = default!;

        public uint? DeliveryId { get; private set; }

        public uint? ManagerId { get; set; }
        public uint? CourierId { get; set; }
    }
}
