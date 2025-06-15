using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;

namespace ShopServices.Core.Services
{
    public interface IPricesService
    {
        public Task<Result> AddPrice(Price price);
        Task<Price> GetPriceById(uint priceId);

        public Task<IEnumerable<Price>> GetPricesForProduct(
            uint productId,
            DateTime actualFromDt,
            DateTime? actualToDt,
            uint byPage = 10,
            uint page = 1);
        
        public Task<Result> UpdateActualToDt(
            uint priceId,
            DateTime? actualToDt);
    }
}
