using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;

namespace ShopServices.Core.Repositories
{
    public interface IOrdersRepository
    {
        public Task<Result> Create(Order newOrder);
        public Task<Order> GetOrderInfoById(uint orderId);
        public Task<Result> UpdateShopIdByBuyer(uint buyerId, uint orderId, uint? shopId);
    }
}
