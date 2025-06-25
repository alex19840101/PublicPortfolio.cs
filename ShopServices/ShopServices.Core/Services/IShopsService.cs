using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;

namespace ShopServices.Core.Services
{
    public interface IShopsService
    {
        /// <summary> Добавление магазина </summary>
        public Task<Result> AddShop(Shop shop);
    }
}
