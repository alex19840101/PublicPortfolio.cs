using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;
using ShopServices.Core.Repositories;
using ShopServices.Core.Services;

namespace ShopServices.BusinessLogic
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _deliveryRepository;

        public DeliveryService(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        public async Task<Result> AddDelivery(Delivery delivery)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Delivery>> GetDeliveries(
            uint? regionCode,
            string addressSubString = null,
            uint byPage = 10,
            uint page = 1,
            bool ignoreCase = true)
        {
            throw new NotImplementedException();
        }

        public async Task<Delivery> GetDeliveryById(uint deliveryId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> UpdateDelivery(Delivery delivery)
        {
            throw new NotImplementedException();
        }
        public async Task<Result> ArchiveDelivery(uint deliveryId)
        {
            throw new NotImplementedException();
        }
    }
}
