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
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehousesRepository _warehousesRepository;

        public WarehouseService(IWarehousesRepository warehousesRepository)
        {
            _warehousesRepository = warehousesRepository;
        }

        public async Task<Result> AddWarehouse(Warehouse warehouse)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> ArchiveWarehouse(uint warehouseId)
        {
            throw new NotImplementedException();
        }

        public async Task<Warehouse> GetWarehouseById(uint warehouseId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> UpdateWarehouse(Warehouse warehouse)
        {
            throw new NotImplementedException();
        }
    }
}
