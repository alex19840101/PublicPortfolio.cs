using System.Collections.Generic;
using System.Linq;
using ShopServices.Core.Models;

namespace Goods.API
{
    internal static class ProductMapper
    {
        internal static Contracts.Product GetProductDto(Product coreProduct)
        {
            return new Contracts.Product
            {
                Id = coreProduct.Id,
                ArticleNumber = coreProduct.ArticleNumber,
                Brand = coreProduct.Brand,
                Name = coreProduct.Name,
                Params = coreProduct.Params,
                Url = coreProduct.Url,
                ImageUrl = coreProduct.ImageUrl,
                GoodsGroups = coreProduct.GoodsGroups,
                Archieved = coreProduct.Archieved
            };
        }

        internal static Product GetCoreProduct(Contracts.Product productDto)
        {
            return new Product(
                id: productDto.Id,
                articleNumber: productDto.ArticleNumber,
                brand: productDto.Brand,
                name: productDto.Name,
                parameters: productDto.Params,
                url: productDto.Url,
                imageUrl: productDto.ImageUrl,
                goodsGroups: productDto.GoodsGroups,
                archieved: productDto.Archieved);
        }

        /// <summary>
        /// Маппинг IEnumerable(Core.Models.Product) - IEnumerable(Contracts.Product)
        /// </summary>
        /// <param name="productsList"> список товаров IEnumerable(Core.Models.Product) </param>
        /// <returns></returns>
        internal static IEnumerable<Contracts.Product> GetProductDtos(this IEnumerable<Product> productsList)
        {
            return productsList.Select(coreProduct => new Contracts.Product
            {
                Id = coreProduct.Id,
                ArticleNumber = coreProduct.ArticleNumber,
                Brand = coreProduct.Brand,
                Name = coreProduct.Name,
                Params = coreProduct.Params,
                Url = coreProduct.Url,
                ImageUrl = coreProduct.ImageUrl,
                GoodsGroups = coreProduct.GoodsGroups,
                Archieved = coreProduct.Archieved
            });
        }
    }
}
