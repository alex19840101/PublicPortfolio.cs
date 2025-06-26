using System;
using System.Collections.Generic;
using System.Linq;
using Shops.API.Contracts.Requests;
using ShopServices.Core.Models;

namespace Shops.API
{
    internal static class ShopsMapper
    {
        internal static Shop PrepareCoreShop(AddShopRequestDto addShopRequestDto)
        {
            return new Shop(
                id: 0,
                name: addShopRequestDto.Name,
                regionCode: addShopRequestDto.RegionCode,
                address: addShopRequestDto.Address,
                phone: addShopRequestDto.Phone,
                email: addShopRequestDto.Email,
                url: addShopRequestDto.Url,
                archived: false,
                workSchedule: addShopRequestDto.WorkSchedule);
        }

        internal static Contracts.Responses.ShopResponseDto GetShopDto(Shop coreShop)
        {
            return new Contracts.Responses.ShopResponseDto
            {
                Id = coreShop.Id,
                Name = coreShop.Name,
                RegionCode = coreShop.RegionCode,
                Address = coreShop.Address,
                Phone = coreShop.Phone,
                Email = coreShop.Email,
                Url = coreShop.Url,
                CreatedDt = coreShop.CreatedDt!.Value,
                UpdatedDt = coreShop.Updated,
                WorkSchedule = coreShop.WorkSchedule
            };
        }

        internal static Shop GetCoreShop(UpdateShopRequestDto updateShopRequestDto)
        {
            return new Shop(
                id: updateShopRequestDto.Id,
                name: updateShopRequestDto.Name,
                regionCode: updateShopRequestDto.RegionCode,
                address: updateShopRequestDto.Address,
                phone: updateShopRequestDto.Phone,
                email: updateShopRequestDto.Email,
                url: updateShopRequestDto.Url,
                workSchedule: updateShopRequestDto.WorkSchedule);
        }


        /// <summary>
        /// Маппинг IEnumerable(Core.Models.Shop) - IEnumerable(Contracts.Responses.ShopResponseDto)
        /// </summary>
        /// <param name="shopsList"> список магазинов IEnumerable(Core.Models.Shop) </param>
        /// <returns></returns>
        internal static IEnumerable<Contracts.Responses.ShopResponseDto> GetShopDtos(this IEnumerable<Shop> shopsList)
        {
            return shopsList.Select(coreShop => new Contracts.Responses.ShopResponseDto
            {
                Id = coreShop.Id,
                Name = coreShop.Name,
                RegionCode = coreShop.RegionCode,
                Address = coreShop.Address,
                Phone = coreShop.Phone,
                Email = coreShop.Email,
                Url = coreShop.Url,
                CreatedDt = coreShop.CreatedDt!.Value,
                UpdatedDt = coreShop.Updated,
                WorkSchedule = coreShop.WorkSchedule
            });
        }
    }
}
