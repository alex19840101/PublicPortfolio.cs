using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;

namespace ShopServices.Core.Repositories
{
    public interface IDeliveryRepository
    {
        public Task<Result> Create(Delivery newDelivery);
        public Task<Delivery> GetDeliveryById(uint deliveryId);
        public Task<IEnumerable<Delivery>> GetDeliveries(
            uint? regionCode,
            string addressSubString,
            uint? buyerId,
            uint take,
            uint skip,
            bool ignoreCase = true);
        Task<IEnumerable<Delivery>> GetDeliveriesForOrder(
            uint orderId,
            uint take,
            uint skip);
        public Task<Result> UpdateDelivery(Delivery delivery);
        public Task<Result> ArchiveDeliveryById(uint deliveryId);
    }
}
