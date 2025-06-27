using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core;
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

        public async Task<Result> AddWarehouse(Warehouse newWarehouse)
        {
            if (newWarehouse == null)
                throw new ArgumentNullException(ResultMessager.NEW_WAREHOUSE_RARAM_NAME);

            if (string.IsNullOrWhiteSpace(newWarehouse.Name))
                return new Result(ResultMessager.NAME_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            var existingCategory = await _warehousesRepository.GetWarehouseByAddress(newWarehouse.Address);

            if (existingCategory != null)
            {
                if (!existingCategory.IsEqualIgnoreIdAndDt(newWarehouse))
                    return new Result(ResultMessager.CONFLICT, System.Net.HttpStatusCode.Conflict);

                return new Result(ResultMessager.ALREADY_EXISTS, System.Net.HttpStatusCode.Created, id: existingCategory.Id);
            }

            var createResult = await _warehousesRepository.Create(newWarehouse);

            return createResult;
        }

        public async Task<Result> ArchiveWarehouse(uint warehouseId)
        {
            return await _warehousesRepository.ArchiveWarehouseById(warehouseId);
        }

        public async Task<Warehouse> GetWarehouseById(uint warehouseId)
        {
            return await _warehousesRepository.GetWarehouseById(warehouseId);
        }

        public async Task<IEnumerable<Warehouse>> GetWarehouses(
            uint? regionCode = null,
            string nameSubString = null,
            string addressSubString = null,
            uint byPage = 10,
            uint page = 1,
            bool ignoreCase = true)
        {
            var take = byPage;
            var skip = page > 1 ? (page - 1) * byPage : 0;

            return await _warehousesRepository.GetWarehouses(
                regionCode: regionCode,
                nameSubString: nameSubString,
                addressSubString: addressSubString,
                take: take,
                skipCount: skip,
                ignoreCase: ignoreCase);
        }

        public async Task<Result> UpdateWarehouse(Warehouse warehouse)
        {
            if (warehouse == null)
                throw new ArgumentNullException(ResultMessager.WAREHOUSE_RARAM_NAME);

            if (string.IsNullOrWhiteSpace(warehouse.Name))
                return new Result(ResultMessager.NAME_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(warehouse.Address))
                return new Result(ResultMessager.ADDRESS_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(warehouse.Url))
                return new Result(ResultMessager.URL_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(warehouse.Phone))
                return new Result(ResultMessager.PHONE_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(warehouse.Email))
                return new Result(ResultMessager.EMAIL_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(warehouse.WorkSchedule))
                return new Result(ResultMessager.WORK_SCHEDULE_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            return await _warehousesRepository.UpdateWarehouse(warehouse);
        }
    }
}
