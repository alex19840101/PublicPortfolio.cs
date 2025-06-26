using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;

namespace ShopServices.Core.Services
{
    public interface IDeliveryService
    {
        public Task<Result> AddDelivery(Delivery delivery);
        public Task<Delivery> GetDeliveryById(uint deliveryId);
        public Task<IEnumerable<Delivery>> GetDeliveries(
            uint? regionCode,
            string addressSubString = null,
            uint byPage = 10,
            uint page = 1,
            bool ignoreCase = true);
        public Task<Result> UpdateDelivery(Delivery delivery);
        public Task<Result> ArchiveDelivery(uint deliveryId);
    }
}
