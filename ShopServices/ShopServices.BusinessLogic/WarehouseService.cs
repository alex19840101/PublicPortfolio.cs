using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
