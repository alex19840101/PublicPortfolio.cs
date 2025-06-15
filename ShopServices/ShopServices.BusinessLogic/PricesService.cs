using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Models;
using ShopServices.Core.Repositories;
using ShopServices.Core.Services;

namespace ShopServices.BusinessLogic
{
    public class PricesService : IPricesService
    {
        private readonly IPricesRepository _pricesRepository;
        private readonly IProductsRepository _productsRepository;

        public PricesService(
            IPricesRepository pricesRepository,
            IProductsRepository productsRepository)
        {
            _pricesRepository = pricesRepository;
            _productsRepository = productsRepository;
        }

        public async Task<Result> AddPrice(Price newPrice)
        {
            if (newPrice == null)
                throw new ArgumentNullException(ResultMessager.NEWPRICE_RARAM_NAME);

            var product = await _productsRepository.GetProductById(newPrice.ProductId);

            if (product == null)
                return new Result(ResultMessager.PRODUCT_NOT_FOUND, System.Net.HttpStatusCode.NotFound);

            var unValidatedPriceResult = UnValidatedPriceResult(newPrice);
            if (unValidatedPriceResult != null)
                return unValidatedPriceResult;

            var existingPrices = await _pricesRepository.GetPricesForProduct(
               productId: newPrice.ProductId,
               actualFromDt: newPrice.ActualFromDt,
               actualToDt: newPrice.ActualToDt);


            if (existingPrices.Any(p => p.ActualFromDt == newPrice.ActualFromDt || p.ActualToDt == newPrice.ActualToDt))
            {
                return new Result(ResultMessager.CONFLICT, System.Net.HttpStatusCode.Conflict);
            }

            var createResult = await _pricesRepository.Create(newPrice);

            return createResult;
        }

        public async Task<Price> GetPriceById(uint priceId)
        {
            return await _pricesRepository.GetPriceById(priceId);
        }

        public async Task<IEnumerable<Price>> GetPricesForProduct(
            uint productId,
            DateTime actualFromDt,
            DateTime? actualToDt,
            uint byPage = 10,
            uint page = 1)
        {
            var take = byPage;
            var skip = page > 1 ? (page - 1) * byPage : 0;

            return await _pricesRepository.GetPricesForProduct(
            productId: productId,
            actualFromDt: actualFromDt,
            actualToDt: actualToDt,
            take: byPage,
            skipCount: skip);
        }

        public async Task<Result> UpdateActualToDt(
            uint priceId,
            DateTime? actualToDt)
        {
            return await _pricesRepository.UpdateActualToDt(priceId, actualToDt);
        }

        /// <summary>
        /// Валидация полей ценника Price
        /// </summary>
        /// <param name="price"> Price - ценник </param>
        /// <returns> Result - при ошибке | null, если проверка пройдена </returns>
        private Result UnValidatedPriceResult(Price price)
        {
            if (price.PricePerUnit <= 0)
                return new Result(ResultMessager.PRICEPERUNIT_SHOULD_BE_POSITIVE, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(price.Currency))
                return new Result(ResultMessager.CURRENCY_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(price.Unit))
                return new Result(ResultMessager.UNIT_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (price.ActualFromDt < DateTime.Now)
                return new Result(ResultMessager.ACTUALFROMDT_SHOULD_BE_GE_NOW, System.Net.HttpStatusCode.BadRequest);

            if (price.ActualToDt == null)
                return null;

            if (price.ActualToDt < DateTime.Now)
                return new Result(ResultMessager.ACTUALTODT_SHOULD_BE_GE_NOW, System.Net.HttpStatusCode.BadRequest);

            if (price.ActualFromDt >= price.ActualToDt)
                return new Result(ResultMessager.ACTUALFROMDT_SHOULD_BE_LESS_THAN_ACTUALTODT, System.Net.HttpStatusCode.BadRequest);

            return null;
        }
    }
}
