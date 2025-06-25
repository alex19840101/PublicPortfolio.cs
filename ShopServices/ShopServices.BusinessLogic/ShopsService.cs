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
    public class ShopsService : IShopsService
    {
        private readonly IShopsRepository _shopsRepository;

        public ShopsService(IShopsRepository shopsRepository)
        {
            _shopsRepository = shopsRepository;
        }

        public async Task<Result> AddShop(Shop shop)
        {
            throw new NotImplementedException();
        }
    }
}
