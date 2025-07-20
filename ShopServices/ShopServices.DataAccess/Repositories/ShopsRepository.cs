using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    public class ShopsRepository : IShopsRepository
    {
        private readonly ShopServicesDbContext _dbContext;

        public ShopsRepository(ShopServicesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> ArchiveShopById(uint shopId)
        {
            var entityShop = await _dbContext.Shops
                .SingleOrDefaultAsync(s => s.Id == shopId);

            if (entityShop is null)
                return new Result(ResultMessager.NOT_FOUND, HttpStatusCode.NotFound);

            if (!entityShop.Archived)
            {
                entityShop.Archive();
                entityShop.UpdateUpdatedDt(DateTime.Now.ToUniversalTime());

                await _dbContext.SaveChangesAsync();
            }

            return new Result(ResultMessager.OK, HttpStatusCode.OK);
        }

        public async Task<Result> Create(Shop newShop)
        {
            ArgumentNullException.ThrowIfNull(newShop);

            var newShopEntity = new Entities.Shop(
                id: 0,
                name: newShop.Name,
                regionCode: newShop.RegionCode,
                address: newShop.Address,
                phone: newShop.Phone,
                email: newShop.Email,
                url: newShop.Url,
                workSchedule: newShop.WorkSchedule,
                createdDt: DateTime.Now.ToUniversalTime());
            
            await _dbContext.Shops.AddAsync(newShopEntity);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(newShopEntity).GetDatabaseValuesAsync(); //получение сгенерированного БД id
            return new Result
            {
                Id = newShopEntity.Id,
                StatusCode = HttpStatusCode.Created,
                Message = ResultMessager.OK
            };
        }

        public async Task<Shop?> GetShopByAddress(string address)
        {
            var query = _dbContext.Shops.AsNoTracking().Where(s => EF.Functions.Like(s.Address.ToLower(), $"%{address.ToLower()}%"));
            
            var shopEntity = await query.SingleOrDefaultAsync();
            
            if (shopEntity is null)
                return null;

            return GetCoreShop(shopEntity);
        }

        public async Task<Shop?> GetShopById(uint shopId)
        {
            var shopEntity = await GetShopEntity(shopId, asNoTracking: true);
            if (shopEntity is null)
                return null;

            return GetCoreShop(shopEntity);
        }

        public async Task<IEnumerable<Shop>> GetShops(
            uint? regionCode,
            string nameSubString,
            string addressSubStr,
            uint take,
            uint skipCount,
            bool ignoreCase = true)
        {
            var limitCount = take > 100 ? 100 : take;
            List<Entities.Shop> entityShopsLst;

            if (string.IsNullOrWhiteSpace(addressSubStr) && string.IsNullOrWhiteSpace(nameSubString))
            {
                entityShopsLst = regionCode == null ?
                    await _dbContext.Shops.AsNoTracking()
                    .Skip((int)skipCount).Take((int)limitCount).ToListAsync() :
                    
                    await _dbContext.Shops.AsNoTracking().Where(s => s.RegionCode == regionCode)
                    .Skip((int)skipCount).Take((int)limitCount).ToListAsync();

                if (entityShopsLst.Count == 0)
                    return [];

                return entityShopsLst.Select(s => GetCoreShop(s));
            }
            Expression<Func<Entities.Shop, bool>> expressionWhereName = ignoreCase ?
                s => EF.Functions.Like(s.Name.ToLower(), $"%{nameSubString.ToLower()}%") :
                s => s.Name.Contains(nameSubString);

            if (string.IsNullOrWhiteSpace(addressSubStr))
            {
                entityShopsLst = await _dbContext.Shops
                        .AsNoTracking()
                        .Where(s => s.RegionCode == regionCode)
                        .Where(expressionWhereName).Skip((int)skipCount).Take((int)limitCount).ToListAsync();

                if (entityShopsLst.Count == 0)
                    return [];

                return entityShopsLst.Select(s => GetCoreShop(s));
            }

            //addressSubStr задан
            Expression<Func<Entities.Shop, bool>> expressionWhereAddress = ignoreCase ?
                s => EF.Functions.Like(s.Address!.ToLower(), $"%{addressSubStr.ToLower()}%") :
                s => s.Address!.Contains(addressSubStr);

            entityShopsLst = string.IsNullOrWhiteSpace(nameSubString) ?
                await _dbContext.Shops
                        .AsNoTracking()
                        .Where(s => s.RegionCode == regionCode)
                        .Where(expressionWhereAddress).Skip((int)skipCount).Take((int)limitCount).ToListAsync() :
                await _dbContext.Shops
                        .AsNoTracking()
                        .Where(s => s.RegionCode == regionCode)
                        .Where(expressionWhereAddress)
                        .Where(expressionWhereName).Skip((int)skipCount).Take((int)limitCount)
                        .ToListAsync();

            if (entityShopsLst.Count == 0)
                return [];

            return entityShopsLst.Select(s => GetCoreShop(s));
        }

        public async Task<Result> UpdateShop(Shop upd)
        {
            ArgumentNullException.ThrowIfNull(upd);

            var shopEntity = await _dbContext.Shops
                .SingleOrDefaultAsync(s => s.Id == upd.Id);

            if (shopEntity is null)
                return new Result(ResultMessager.NOT_FOUND, HttpStatusCode.NotFound);

            if (!string.Equals(upd.Name, shopEntity.Name)) shopEntity.UpdateName(upd.Name);
            if (upd.RegionCode != shopEntity.RegionCode) shopEntity.UpdateRegionCode(upd.RegionCode);
            if (!string.Equals(upd.Address, shopEntity.Address)) shopEntity.UpdateAddress(upd.Address);
            if (!string.Equals(upd.Phone, shopEntity.Phone)) shopEntity.UpdatePhone(upd.Phone);
            if (!string.Equals(upd.Email, shopEntity.Email)) shopEntity.UpdateEmail(upd.Email);
            if (!string.Equals(upd.Url, shopEntity.Url)) shopEntity.UpdateUrl(upd.Url);
            if (!string.Equals(upd.WorkSchedule, shopEntity.WorkSchedule)) shopEntity.UpdateWorkSchedule(upd.WorkSchedule);

            if (_dbContext.ChangeTracker.HasChanges())
            {
                shopEntity.UpdateUpdatedDt( DateTime.Now.ToUniversalTime());
                await _dbContext.SaveChangesAsync();
                return new Result(ResultMessager.SHOP_UPDATED, HttpStatusCode.OK);
            }

            return new Result(ResultMessager.SHOP_IS_ACTUAL, HttpStatusCode.OK);
        }

        private async Task<Entities.Shop?> GetShopEntity(uint id, bool asNoTracking)
        {
            var query = asNoTracking ?
                _dbContext.Shops.AsNoTracking().Where(s => s.Id == id) :
                _dbContext.Shops.Where(s => s.Id == id);

            var shopEntity = await query.SingleOrDefaultAsync();

            return shopEntity;
        }


        private static Shop GetCoreShop(Entities.Shop shopEntity) =>
            new Shop(
                id: shopEntity.Id,
                name: shopEntity.Name,
                regionCode: shopEntity.RegionCode,
                address: shopEntity.Address,
                phone: shopEntity.Phone,
                email: shopEntity.Email,
                url: shopEntity.Url,
                createdDt: shopEntity.CreatedDt!.Value.ToLocalTime(),
                updated: shopEntity.Updated?.ToLocalTime(),
                archived: shopEntity.Archived,
                workSchedule: shopEntity.WorkSchedule);
    }
}
