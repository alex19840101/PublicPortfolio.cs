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
    public class AvailabilityRepository : IAvailabilityRepository
    {
        private readonly ShopServicesDbContext _dbContext;

        public AvailabilityRepository(ShopServicesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Create(Availability newAvailability)
        {
            ArgumentNullException.ThrowIfNull(newAvailability);

            var newAvailabilityEntity = new Entities.Availability
            (
                id: newAvailability.Id,
                productId: newAvailability.ProductId,
                cityTownCode: newAvailability.CityTownCode,
                shopId: newAvailability.ShopId,
                warehouseId: newAvailability.WarehouseId,
                count: newAvailability.Count,
                managerId: newAvailability.ManagerId,
                placeName: newAvailability.PlaceName,
                created: DateTime.Now.ToUniversalTime(),
                updated: null,
                nextSupplyTime: newAvailability.NextSupplyTime,
                lastSupplyTime: newAvailability.LastSupplyTime
            );

            await _dbContext.Availabilities.AddAsync(newAvailabilityEntity);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(newAvailabilityEntity).GetDatabaseValuesAsync(); //получение сгенерированного БД id
            return new Result
            {
                Id = newAvailabilityEntity.Id,
                StatusCode = HttpStatusCode.Created,
                Message = ResultMessager.OK
            };
        }

        public async Task<Result> DeleteAvailability(ulong availabilityId)
        {
            var entityAvailability = await _dbContext.Availabilities
                .SingleOrDefaultAsync(a => a.Id == availabilityId);

            if (entityAvailability is null)
                return new Result(ResultMessager.NOT_FOUND, HttpStatusCode.NotFound);

            _dbContext.Availabilities.Remove(entityAvailability);

            await _dbContext.SaveChangesAsync();

            return new Result(ResultMessager.OK, HttpStatusCode.OK);
        }

        public async Task<IEnumerable<Availability>> GetAvailabilitiesByProductId(uint productId)
        {
            var entityAvailabilities = await _dbContext.Availabilities
                .Where(a => a.ProductId == productId).ToListAsync();

            return entityAvailabilities.Select(a => GetCoreAvailability(a));
        }

        public async Task<Availability?> GetAvailabilityById(ulong availabilityId)
        {
            var entityAvailability = await _dbContext.Availabilities
                .SingleOrDefaultAsync(a => a.Id == availabilityId);

            if (entityAvailability is null)
                return null;

            return GetCoreAvailability(entityAvailability);
        }

        public async Task<Result> UpdateAvailability(Availability upd)
        {
            ArgumentNullException.ThrowIfNull(upd);

            var availabilityEntity = await _dbContext.Availabilities
                .SingleOrDefaultAsync(a => a.Id == upd.Id);

            if (availabilityEntity is null)
                return new Result(ResultMessager.NOT_FOUND, HttpStatusCode.NotFound);

            if (!string.Equals(upd.PlaceName, availabilityEntity.PlaceName)) availabilityEntity.UpdatePlaceName(placeName: upd.PlaceName);
            if (upd.Updated != availabilityEntity.Updated) availabilityEntity.UpdateUpdated(upd.Updated);
            if (upd.Count != availabilityEntity.Count) availabilityEntity.UpdateCount(upd.Count);
            if (upd.NextSupplyTime != availabilityEntity.NextSupplyTime) availabilityEntity.UpdateNextSupplyTime(upd.NextSupplyTime);
            if (upd.LastSupplyTime != availabilityEntity.LastSupplyTime) availabilityEntity.UpdateLastSupplyTime(upd.LastSupplyTime);
            if (upd.ManagerId != availabilityEntity.ManagerId) availabilityEntity.UpdateManagerId(upd.ManagerId);


            if (_dbContext.ChangeTracker.HasChanges())
            {
                availabilityEntity.UpdateUpdated(DateTime.Now.ToUniversalTime());
                await _dbContext.SaveChangesAsync();
                return new Result(ResultMessager.AVAILABILITY_UPDATED, HttpStatusCode.OK);
            }

            return new Result(ResultMessager.AVAILABILITY_IS_ACTUAL, HttpStatusCode.OK);
        }

        private static Availability GetCoreAvailability(Entities.Availability availabilitytEntity) =>
            new Availability(
                id: availabilitytEntity.Id,
                productId: availabilitytEntity.ProductId,
                cityTownCode: availabilitytEntity.CityTownCode,
                shopId: availabilitytEntity.ShopId,
                warehouseId: availabilitytEntity.WarehouseId,
                count: availabilitytEntity.Count,
                managerId: availabilitytEntity.ManagerId,
                placeName: availabilitytEntity.PlaceName,
                created: availabilitytEntity.Created,
                updated: availabilitytEntity.Updated,
                nextSupplyTime: availabilitytEntity.NextSupplyTime,
                lastSupplyTime: availabilitytEntity.LastSupplyTime);
    }
}
