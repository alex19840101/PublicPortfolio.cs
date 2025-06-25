using System;
using System.Collections.Generic;
using System.Linq;
using ShopServices.Core.Models;
using Warehouses.API.Contracts.Requests;
using Warehouses.API.Contracts.Responses;

namespace Warehouses.API
{
    internal static class WarehousesMapper
    {
        internal static Warehouse PrepareCoreWarehouse(AddWarehouseRequestDto addWarehouseRequestDto)
        {
            return new Warehouse(
                id: 0,
                name: addWarehouseRequestDto.Name,
                address: addWarehouseRequestDto.Address,
                phone: addWarehouseRequestDto.Phone,
                email: addWarehouseRequestDto.Email,
                url: addWarehouseRequestDto.Url);
        }

        internal static Contracts.Responses.WarehouseResponseDto GetWarehouseDto(Warehouse coreWarehouse)
        {
            return new Contracts.Responses.WarehouseResponseDto
            {
                Id = coreWarehouse.Id,
                Name = coreWarehouse.Name,
                Address = coreWarehouse.Address,
                Phone = coreWarehouse.Phone,
                Email = coreWarehouse.Email,
                Url = coreWarehouse.Url,
                CreatedDt = coreWarehouse.CreatedDt!.Value,
                UpdatedDt = coreWarehouse.Updated
            };
        }


        /// <summary>
        /// Маппинг IEnumerable(Core.Models.Warehouse) - IEnumerable(Contracts.Responses.WarehouseResponseDto)
        /// </summary>
        /// <param name="warehousesList"> список складов IEnumerable(Core.Models.Warehouse) </param>
        /// <returns></returns>
        internal static IEnumerable<Contracts.Responses.WarehouseResponseDto> GetWarehouseDtos(this IEnumerable<Warehouse> warehousesList)
        {
            return warehousesList.Select(coreWarehouse => new Contracts.Responses.WarehouseResponseDto
            {
                Id = coreWarehouse.Id,
                Name = coreWarehouse.Name,
                Address = coreWarehouse.Address,
                Phone = coreWarehouse.Phone,
                Email = coreWarehouse.Email,
                Url = coreWarehouse.Url,
                CreatedDt = coreWarehouse.CreatedDt!.Value,
                UpdatedDt = coreWarehouse.Updated
            });
        }
    }
}
