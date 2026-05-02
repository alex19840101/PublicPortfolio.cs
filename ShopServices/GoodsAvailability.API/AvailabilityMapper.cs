using System.Collections.Generic;
using System.Linq;
using ShopServices.Core.Models;

namespace GoodsAvailability.API
{
    internal static class AvailabilityMapper
    {
        internal static Contracts.Availability GetAvailabilityDto(Availability coreAvailability)
        {
            return new Contracts.Availability
            {
                Id = coreAvailability.Id,
                ProductId = coreAvailability.ProductId,
                ShopId = coreAvailability.ShopId,
                WarehouseId = coreAvailability.WarehouseId,
                Count = coreAvailability.Count,
                ManagerId = coreAvailability.ManagerId,
                PlaceName = coreAvailability.PlaceName,
                CityTownCode = coreAvailability.CityTownCode,
                Created = coreAvailability.Created,
                Updated = coreAvailability.Updated,
                NextSupplyTime = coreAvailability.NextSupplyTime,
                LastSupplyTime = coreAvailability.LastSupplyTime
            };
        }

        internal static Availability GetCoreAvailability(Contracts.Availability availabilityDto)
        {
            return new Availability(
                id: availabilityDto.Id,
                productId: availabilityDto.ProductId,
                cityTownCode: availabilityDto.CityTownCode,
                shopId: availabilityDto.ShopId,
                warehouseId: availabilityDto.WarehouseId,
                count: availabilityDto.Count,
                managerId: availabilityDto.ManagerId,
                placeName: availabilityDto.PlaceName,
                created: availabilityDto.Created,
                updated: availabilityDto.Updated,
                nextSupplyTime: availabilityDto.NextSupplyTime,
                lastSupplyTime: availabilityDto.LastSupplyTime);
        }

        /// <summary>
        /// Маппинг IEnumerable(Core.Models.Availability) - IEnumerable(Contracts.Availability)
        /// </summary>
        /// <param name="availabilitiesList"> список товаров IEnumerable(Core.Models.Availability) </param>
        /// <returns></returns>
        internal static IEnumerable<Contracts.Availability> GetAvailabilityDtos(this IEnumerable<Availability> availabilitiesList)
        {
            return availabilitiesList.Select(coreAvailability => new Contracts.Availability
            {
                Id = coreAvailability.Id,
                ProductId = coreAvailability.ProductId,
                CityTownCode = coreAvailability.CityTownCode,
                ShopId = coreAvailability.ShopId,
                WarehouseId = coreAvailability.WarehouseId,
                Count = coreAvailability.Count,
                ManagerId = coreAvailability.ManagerId,
                PlaceName = coreAvailability.PlaceName,
                Created = coreAvailability.Created,
                Updated = coreAvailability.Updated,
                NextSupplyTime = coreAvailability.NextSupplyTime,
                LastSupplyTime = coreAvailability.LastSupplyTime
            });
        }
    }
}
