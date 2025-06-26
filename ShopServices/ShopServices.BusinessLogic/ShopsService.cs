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
    public class ShopsService : IShopsService
    {
        private readonly IShopsRepository _shopsRepository;

        public ShopsService(IShopsRepository shopsRepository)
        {
            _shopsRepository = shopsRepository;
        }

        public async Task<Result> AddShop(Shop newShop)
        {
            if (newShop == null)
                throw new ArgumentNullException(ResultMessager.NEW_SHOP_RARAM_NAME);

            if (string.IsNullOrWhiteSpace(newShop.Name))
                return new Result(ResultMessager.NAME_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            var existingShop = await _shopsRepository.GetShopByAddress(newShop.Address);

            if (existingShop != null)
            {
                if (!existingShop.IsEqualIgnoreIdAndDt(newShop))
                    return new Result(ResultMessager.CONFLICT, System.Net.HttpStatusCode.Conflict);

                return new Result(ResultMessager.ALREADY_EXISTS, System.Net.HttpStatusCode.Created, id: existingShop.Id);
            }

            var createResult = await _shopsRepository.Create(newShop);

            return createResult;
        }

        public async Task<Result> ArchiveShop(uint shopId)
        {
            return await _shopsRepository.ArchiveShopById(shopId);
        }

        public async Task<Shop> GetShopById(uint shopId)
        {
            return await _shopsRepository.GetShopById(shopId);
        }

        public async Task<IEnumerable<Shop>> GetShops(
            uint? regionCode = null,
            string nameSubString = null,
            string addressSubString = null,
            uint byPage = 10,
            uint page = 1,
            bool ignoreCase = true)
        {
            var take = byPage;
            var skip = page > 1 ? (page - 1) * byPage : 0;

            return await _shopsRepository.GetShops(
                regionCode: regionCode,
                nameSubString: nameSubString,
                addressSubString: addressSubString,
                take: take,
                skipCount: skip);
        }

        public async Task<Result> UpdateShop(Shop shop)
        {
            if (shop == null)
                throw new ArgumentNullException(ResultMessager.SHOP_RARAM_NAME);

            if (string.IsNullOrWhiteSpace(shop.Name))
                return new Result(ResultMessager.NAME_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(shop.Address))
                return new Result(ResultMessager.ADDRESS_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(shop.Url))
                return new Result(ResultMessager.URL_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(shop.Phone))
                return new Result(ResultMessager.PHONE_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(shop.Email))
                return new Result(ResultMessager.EMAIL_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(shop.WorkSchedule))
                return new Result(ResultMessager.WORK_SCHEDULE_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            return await _shopsRepository.UpdateShop(shop);
        }
    }
}
