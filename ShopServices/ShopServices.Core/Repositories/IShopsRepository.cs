using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;

namespace ShopServices.Core.Repositories
{
    public interface IShopsRepository
    {
        public Task<Result> Create(Shop newShop);
        public Task<Shop> GetShopByAddress(string address);
        public Task<Shop> GetShopById(uint shopId);
        public Task<Result> UpdateShop(Shop shop);
        public Task<Result> ArchiveShopById(uint shopId);
    }
}
