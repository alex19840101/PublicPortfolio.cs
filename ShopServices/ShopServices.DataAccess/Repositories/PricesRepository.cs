using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Models;
using ShopServices.Core.Repositories;

namespace ShopServices.DataAccess.Repositories
{
    public class PricesRepository : IPricesRepository
    {
        private readonly ShopServicesDbContext _dbContext;

        public PricesRepository(ShopServicesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Create(Price newPrice)
        {
            ArgumentNullException.ThrowIfNull(newPrice);

            var productEntity = _dbContext.Products.AsNoTracking()
                .Where(product => product.Id == newPrice.ProductId);

            if (productEntity is null)
                return new Result(ResultMessager.NOT_FOUND, HttpStatusCode.NotFound);

            var newPriceEntity = new Entities.Price
            (
                id: newPrice.Id,
                productId: newPrice.ProductId,
                pricePerUnit: newPrice.PricePerUnit,
                currency: newPrice.Currency,
                unit: newPrice.Unit,
                actualFromDt: newPrice.ActualFromDt.ToUniversalTime(),
                actualToDt: newPrice.ActualToDt?.ToUniversalTime()
            );

            await _dbContext.Prices.AddAsync(newPriceEntity);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(newPriceEntity).GetDatabaseValuesAsync(); //получение сгенерированного БД id
            return new Result
            {
                Id = newPriceEntity.Id,
                StatusCode = HttpStatusCode.Created,
                Message = ResultMessager.OK
            };
        }

        public async Task<Price?> GetPriceById(uint priceId)
        {
            var priceEntity = await GetPriceEntity(priceId, asNoTracking: true);
            if (priceEntity is null)
                return null;

            return GetCorePrice(priceEntity);
        }

        public async Task<IEnumerable<Price>> GetPricesForProduct(
            uint productId,
            DateTime actualFromDt,
            DateTime? actualToDt,
            uint take,
            uint skipCount)
        {
            List<Entities.Price> entitiesPrices = await GetIQueryablePricesForProduct(productId, actualFromDt, actualToDt)
                .Skip((int)skipCount)
                .Take((int)take)
                .ToListAsync();

            return entitiesPrices.Select(price => GetCorePrice(price));
        }

        public async Task<IEnumerable<Price>> GetPricesForProduct(
            uint productId,
            DateTime actualFromDt,
            DateTime? actualToDt)
        {
            List<Entities.Price> entitiesPrices = await GetIQueryablePricesForProduct(productId, actualFromDt, actualToDt)
                .ToListAsync();

            return entitiesPrices.Select(price => GetCorePrice(price));
        }

        public async Task<Result> UpdateActualToDt(uint priceId, DateTime? actualToDt)
        {
            var priceEntity = await _dbContext.Prices
                .SingleOrDefaultAsync(price => price.Id == priceId);

            if (priceEntity is null)
                return new Result(ResultMessager.PRICE_NOT_FOUND, HttpStatusCode.NotFound);

            if (actualToDt?.ToUniversalTime() != priceEntity.ActualToDt?.ToLocalTime()) priceEntity.UpdateActualToDt(actualToDt?.ToUniversalTime());

            if (_dbContext.ChangeTracker.HasChanges())
            {
                priceEntity.UpdateUpdatedDt(DateTime.Now.ToUniversalTime());
                await _dbContext.SaveChangesAsync();
                return new Result(ResultMessager.ACTUALTODT_UPDATED, HttpStatusCode.OK);
            }
            return new Result(ResultMessager.ACTUALTODT_IS_ACTUAL, HttpStatusCode.OK);
        }

        private async Task<Entities.Price?> GetPriceEntity(uint priceId, bool asNoTracking)
        {
            var query = asNoTracking ?
                _dbContext.Prices.AsNoTracking().Where(price => price.Id == priceId) :
                _dbContext.Prices.Where(price => price.Id == priceId);

            var priceEntity = await query.SingleOrDefaultAsync();

            return priceEntity;
        }

        private static Price GetCorePrice(Entities.Price priceEntity) =>
            new Price(
            id: priceEntity.Id,
            productId: priceEntity.ProductId,
            pricePerUnit: priceEntity.PricePerUnit,
            currency: priceEntity.Currency,
            unit: priceEntity.Unit,
            actualFromDt: priceEntity.ActualFromDt.ToLocalTime(),
            actualToDt: priceEntity.ActualToDt?.ToLocalTime(),
            updated: priceEntity.Updated?.ToLocalTime());

        private IQueryable<Entities.Price> GetIQueryablePricesForProduct(
            uint productId,
            DateTime actualFromDt,
            DateTime? actualToDt)
        {
            actualFromDt = actualFromDt.ToUniversalTime();
            actualToDt = actualToDt?.ToUniversalTime();

            Expression<Func<Entities.Price, bool>> expressionWhereActualToDt = actualToDt == null ?
                    price => (price.ActualToDt == null || price.ActualToDt != null) :
                    price => price.ActualToDt <= actualToDt;

            return _dbContext.Prices.AsNoTracking()
                .Where(price => price.ProductId == productId)
                .Where(price => price.ActualFromDt >= actualFromDt)
                .Where(expressionWhereActualToDt);
        }
    }
}
