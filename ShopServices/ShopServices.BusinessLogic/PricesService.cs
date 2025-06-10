using System;
using System.Collections.Generic;
using System.Text;
using ShopServices.Core.Repositories;
using ShopServices.Core.Services;

namespace ShopServices.BusinessLogic
{
    public class PricesService : IPricesService
    {
        private readonly IPricesRepository _priceRepository;

        public PricesService(IPricesRepository priceRepository)
        {
            _priceRepository = priceRepository;
        }
    }
}
