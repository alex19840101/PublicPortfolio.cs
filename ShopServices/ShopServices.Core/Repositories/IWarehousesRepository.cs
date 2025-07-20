using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;

namespace ShopServices.Core.Repositories
{
    public interface IWarehousesRepository
    {
        public Task<Result> Create(Warehouse newWarehouse);
        public Task<Warehouse> GetWarehouseByAddress(string address);
        public Task<Warehouse> GetWarehouseById(uint warehouseId);
        public Task<IEnumerable<Warehouse>> GetWarehouses(
            uint? regionCode,
            string nameSubString,
            string addressSubString,
            uint take,
            uint skipCount,
            bool ignoreCase = true);
        public Task<Result> UpdateWarehouse(Warehouse warehouse);
        public Task<Result> ArchiveWarehouseById(uint warehouseId);
    }
}
