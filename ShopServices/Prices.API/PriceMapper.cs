using System.Collections.Generic;
using System.Linq;
using ShopServices.Core.Models;

namespace Prices.API
{
    internal static class PriceMapper
    {
        internal static Contracts.Responses.PriceResponseDto GetPriceDto(Price corePrice)
        {
            return new Contracts.Responses.PriceResponseDto
            {
                Id = corePrice.Id,
                ProductId = corePrice.ProductId,
                PricePerUnit = corePrice.PricePerUnit,
                Currency = corePrice.Currency,
                Unit = corePrice.Unit,
                ActualFromDt = corePrice.ActualFromDt,
                ActualToDt = corePrice.ActualToDt,
                Updated = corePrice.Updated
            };
        }

        internal static Price GetCorePrice(Contracts.Requests.AddPriceRequest productDto)
        {
            return new Price(
                id: 0,
                productId: productDto.ProductId,
                pricePerUnit: productDto.PricePerUnit,
                currency: productDto.Currency,
                unit: productDto.Unit,
                actualFromDt: productDto.ActualFromDt,
                actualToDt: productDto.ActualToDt,
                updated: null);
        }

        /// <summary>
        /// Маппинг IEnumerable(Core.Models.Price) - IEnumerable(Contracts.Responses.PriceResponseDto)
        /// </summary>
        /// <param name="productsList"> список товаров IEnumerable(Core.Models.Price) </param>
        /// <returns></returns>
        internal static IEnumerable<Contracts.Responses.PriceResponseDto> GetPriceDtos(this IEnumerable<Price> productsList)
        {
            return productsList.Select(corePrice => new Contracts.Responses.PriceResponseDto
            {
                Id = corePrice.Id,
                ProductId = corePrice.ProductId,
                PricePerUnit = corePrice.PricePerUnit,
                Currency = corePrice.Currency,
                Unit = corePrice.Unit,
                ActualFromDt = corePrice.ActualFromDt,
                ActualToDt = corePrice.ActualToDt
            });
        }
    }
}
