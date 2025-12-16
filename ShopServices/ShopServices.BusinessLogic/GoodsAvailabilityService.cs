using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Models;
using ShopServices.Core.Services;
using ShopServices.Core.Repositories;
using System.Linq;

namespace ShopServices.BusinessLogic
{
    public class GoodsAvailabilityService : IGoodsAvailabilityService
    {
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly IProductsRepository _productsRepository;
        private readonly IShopsRepository _shopsRepository;
        private readonly IWarehousesRepository _warehousesRepository;

        public GoodsAvailabilityService(
            IAvailabilityRepository availabilityRepository,
            IProductsRepository productsRepository,
            IShopsRepository shopsRepository,
            IWarehousesRepository warehousesRepository
            )
        {
            _availabilityRepository = availabilityRepository;
            _productsRepository = productsRepository;
            _shopsRepository = shopsRepository;
            _warehousesRepository = warehousesRepository;
        }

        public async Task<Result> AddAvailability(Availability newAvailability)
        {
            if (newAvailability == null)
                throw new ArgumentNullException(ResultMessager.NEWAVAILABILITY_RARAM_NAME);

            var unValidatedAvailabilityResult = UnValidatedAvailabilityResult(newAvailability);
            if (unValidatedAvailabilityResult != null)
                return unValidatedAvailabilityResult;

            var existingProductAvailabilities = await _availabilityRepository.GetAvailabilitiesByProductId(newAvailability.ProductId);

            if (existingProductAvailabilities != null && existingProductAvailabilities.Any(
                a => a.WarehouseId == newAvailability.WarehouseId &&
                a.ShopId == newAvailability.ShopId))
            {
                var existingProductAvailability = existingProductAvailabilities.Single(a => a.WarehouseId == newAvailability.WarehouseId &&
                a.ShopId == newAvailability.ShopId);
                if (!existingProductAvailability.IsEqualIgnoreIdAndDt(newAvailability))
                    return new Result(ResultMessager.CONFLICT, System.Net.HttpStatusCode.Conflict);

                return new Result(ResultMessager.ALREADY_EXISTS, System.Net.HttpStatusCode.Created, id: existingProductAvailability.Id);
            }

            var createResult = await _availabilityRepository.Create(newAvailability);

            return createResult;
        }

        public async Task<Result> DeleteAvailability(ulong availabilityId)
        {
            return await _availabilityRepository.DeleteAvailability(availabilityId);
        }

        public async Task<IEnumerable<Availability>> GetAvailabilitiesByProductId(uint productId, uint byPage = 10, uint page = 1)
        {
            throw new NotImplementedException();
        }

        public async Task<Availability> GetAvailabilityById(ulong availabilityId)
        {
            return await _availabilityRepository.GetAvailabilityById(availabilityId);
        }

        public async Task<Result> UpdateAvailability(Availability availability)
        {
            if (availability == null)
                throw new ArgumentNullException(ResultMessager.AVAILABILITY_RARAM_NAME);

            var unValidatedAvailabilityResult = UnValidatedAvailabilityResult(availability);
            if (unValidatedAvailabilityResult != null)
                return unValidatedAvailabilityResult;


            return await _availabilityRepository.UpdateAvailability(availability);
        }

        /// <summary>
        /// Валидация полей Availability
        /// </summary>
        /// <param name="availability"> Availability - наличие товара в магазине/на складе </param>
        /// <returns> Result - при ошибке | null, если проверка пройдена </returns>
        private Result UnValidatedAvailabilityResult(Availability availability)
        {
            if (availability.Id > 0)
                return new Result(ResultMessager.ID_SHOULD_BE_ZERO, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(availability.PlaceName))
                return new Result(ResultMessager.PLACENAME_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (availability.ProductId == 0)
                return new Result(ResultMessager.PRODUCT_ID_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.BadRequest);

            if (availability.ShopId == null && availability.WarehouseId == null)
                return new Result(ResultMessager.SHOP_ID_XOR_WAREHOUSE_ID_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.BadRequest);

            if (availability.ShopId != null && availability.WarehouseId != null)
                return new Result(ResultMessager.SHOP_ID_XOR_WAREHOUSE_ID_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.BadRequest);

            if (availability.ShopId == 0)
                return new Result(ResultMessager.SHOPID_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.BadRequest);

            if (availability.WarehouseId == 0)
                return new Result(ResultMessager.WAREHOUSEID_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.BadRequest);

            if (availability.ManagerId == 0)
                return new Result(ResultMessager.MANAGER_ID_IS_ZERO, System.Net.HttpStatusCode.BadRequest);

            if (availability.Created > DateTime.Now)
                return new Result(ResultMessager.CREATED_IS_MORE_THAN_NOW, System.Net.HttpStatusCode.BadRequest);

            if (availability.Updated > DateTime.Now)
                return new Result(ResultMessager.UPDATED_IS_MORE_THAN_NOW, System.Net.HttpStatusCode.BadRequest);

            if (availability.LastSupplyTime > DateTime.Now)
                return new Result(ResultMessager.LASTSUPPLYTIME_IS_MORE_THAN_NOW, System.Net.HttpStatusCode.BadRequest);

            return null;
        }
    }
}
