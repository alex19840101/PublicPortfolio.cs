using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Models;
using ShopServices.Core.Repositories;

namespace ShopServices.DataAccess.Repositories
{
    public class WarehousesRepository : IWarehousesRepository
    {
        private readonly ShopServicesDbContext _dbContext;
        public WarehousesRepository(ShopServicesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> ArchiveWarehouseById(uint warehouseId)
        {
            var entityWarehouse = await _dbContext.Warehouses
                .SingleOrDefaultAsync(w => w.Id == warehouseId);

            if (entityWarehouse is null)
                return new Result(ResultMessager.NOT_FOUND, HttpStatusCode.NotFound);

            if (!entityWarehouse.Archived)
            {
                entityWarehouse.Archive();

                await _dbContext.SaveChangesAsync();
            }

            return new Result(ResultMessager.OK, HttpStatusCode.OK);
        }

        public async Task<Result> Create(Warehouse newWarehouse)
        {
            ArgumentNullException.ThrowIfNull(newWarehouse);

            var newWarehouseEntity = new Entities.Warehouse(
                id: 0,
                name: newWarehouse.Name,
                address: newWarehouse.Address,
                phone: newWarehouse.Phone,
                email: newWarehouse.Email,
                url: newWarehouse.Url,
                workSchedule: newWarehouse.WorkSchedule);

            await _dbContext.Warehouses.AddAsync(newWarehouseEntity);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(newWarehouseEntity).GetDatabaseValuesAsync(); //получение сгенерированного БД id
            return new Result
            {
                Id = newWarehouseEntity.Id,
                StatusCode = HttpStatusCode.Created,
                Message = ResultMessager.OK
            };
        }

        public async Task<Warehouse?> GetWarehouseByAddress(string address)
        {
            var query = _dbContext.Warehouses.AsNoTracking().Where(s => s.Address.Equals(address, StringComparison.OrdinalIgnoreCase));

            var warehouseEntity = await query.SingleOrDefaultAsync();

            if (warehouseEntity is null)
                return null;

            return GetCoreWarehouse(warehouseEntity);
        }

        public async Task<Warehouse?> GetWarehouseById(uint warehouseId)
        {
            var warehouseEntity = await GetWarehouseEntity(warehouseId, asNoTracking: true);
            if (warehouseEntity is null)
                return null;

            return GetCoreWarehouse(warehouseEntity);
        }

        public async Task<Result> UpdateWarehouse(Warehouse upd)
        {
            ArgumentNullException.ThrowIfNull(upd);

            var warehouseEntity = await _dbContext.Warehouses
                .SingleOrDefaultAsync(s => s.Id == upd.Id);

            if (warehouseEntity is null)
                return new Result(ResultMessager.NOT_FOUND, HttpStatusCode.NotFound);

            if (!string.Equals(upd.Name, warehouseEntity.Name)) warehouseEntity.UpdateName(upd.Name);
            if (!string.Equals(upd.Address, warehouseEntity.Address)) warehouseEntity.UpdateAddress(upd.Address);
            if (!string.Equals(upd.Phone, warehouseEntity.Phone)) warehouseEntity.UpdatePhone(upd.Phone);
            if (!string.Equals(upd.Email, warehouseEntity.Email)) warehouseEntity.UpdateEmail(upd.Email);
            if (!string.Equals(upd.Url, warehouseEntity.Url)) warehouseEntity.UpdateUrl(upd.Url);
            if (!string.Equals(upd.WorkSchedule, warehouseEntity.WorkSchedule)) warehouseEntity.UpdateWorkSchedule(upd.WorkSchedule);

            if (_dbContext.ChangeTracker.HasChanges())
            {
                warehouseEntity.UpdateUpdatedDt(DateTime.Now.ToUniversalTime());
                await _dbContext.SaveChangesAsync();
                return new Result(ResultMessager.WAREHOUSE_UPDATED, HttpStatusCode.OK);
            }

            return new Result(ResultMessager.WAREHOUSE_IS_ACTUAL, HttpStatusCode.OK);
        }

        private async Task<Entities.Warehouse?> GetWarehouseEntity(uint id, bool asNoTracking)
        {
            var query = asNoTracking ?
                _dbContext.Warehouses.AsNoTracking().Where(w => w.Id == id) :
                _dbContext.Warehouses.Where(w => w.Id == id);

            var warehouseEntity = await query.SingleOrDefaultAsync();

            return warehouseEntity;
        }

        private static Warehouse GetCoreWarehouse(Entities.Warehouse warehouseEntity) =>
            new Warehouse(
                id: warehouseEntity.Id,
                name: warehouseEntity.Name,
                address: warehouseEntity.Address,
                phone: warehouseEntity.Phone,
                email: warehouseEntity.Email,
                url: warehouseEntity.Url,
                createdDt: warehouseEntity.CreatedDt!.Value.ToLocalTime(),
                updated: warehouseEntity.Updated?.ToLocalTime(),
                archived: warehouseEntity.Archived,
                workSchedule: warehouseEntity.WorkSchedule);
    }
}
