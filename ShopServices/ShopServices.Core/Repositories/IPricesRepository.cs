using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;

namespace ShopServices.Core.Repositories
{
    public interface IPricesRepository
    {
        public Task<Result> Create(Price newPrice);
        public Task<Price> GetPriceById(uint priceId);
        public Task<IEnumerable<Price>> GetPricesForProduct(
            uint productId,
            DateTime actualFromDt,
            DateTime? actualToDt,
            uint take,
            uint skipCount);
        public Task<IEnumerable<Price>> GetPricesForProduct(
            uint productId,
            DateTime actualFromDt,
            DateTime? actualToDt);
        public Task<Result> UpdateActualToDt
            (uint priceId,
            DateTime? actualToDt);
    }
}
