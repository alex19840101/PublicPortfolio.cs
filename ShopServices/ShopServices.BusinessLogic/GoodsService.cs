using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Models;
using ShopServices.Core.Repositories;
using ShopServices.Core.Services;

namespace ShopServices.BusinessLogic
{
    public class GoodsService : IGoodsService
    {
        private readonly IProductsRepository _productsRepository;

        public GoodsService(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public async Task<Result> AddProduct(Product newProduct)
        {
            if (newProduct == null)
                throw new ArgumentNullException(ResultMessager.NEWPRODUCT_RARAM_NAME);

            var unValidatedProductResult = UnValidatedProductResult(newProduct);
            if (unValidatedProductResult != null)
                return unValidatedProductResult;

            var existingProduct = await _productsRepository.GetProductByName(newProduct.Name);

            if (existingProduct != null)
            {
                if (!existingProduct.IsEqualIgnoreIdAndDt(newProduct))
                    return new Result(ResultMessager.CONFLICT, System.Net.HttpStatusCode.Conflict);

                return new Result(ResultMessager.ALREADY_EXISTS, System.Net.HttpStatusCode.Created, id: existingProduct.Id);
            }

            var createResult = await _productsRepository.Create(newProduct);

            return createResult;
        }

        public async Task<Result> ArchiveProduct(uint id)
        {
            return await _productsRepository.ArchiveProductById(id);
        }

        public async Task<Product> GetProductById(uint id)
        {
            return await _productsRepository.GetProductById(id);
        }

        public async Task<IEnumerable<Product>> GetProducts(
            string nameSubString = null,
            string brandSubStr = null,
            uint byPage = 10,
            uint page = 1,
            bool ignoreCase = true)
        {
            var take = byPage;
            var skip = page > 1 ? (page - 1) * byPage : 0;

            return await _productsRepository.GetProducts(
                nameSubString: nameSubString,
                brandSubStr: brandSubStr,
                take: take,
                skipCount: skip);
        }

        public async Task<IEnumerable<Product>> GetProductsByArticle(
            string articleSubString,
            string brandSubStr = null,
            uint byPage = 10,
            uint page = 1,
            bool ignoreCase = true)
        {
            var take = byPage;
            var skip = page > 1 ? (page - 1) * byPage : 0;

            return await _productsRepository.GetProductsByArticle(
                articleSubString: articleSubString,
                brandSubStr: brandSubStr,
                take: take,
                skipCount: skip);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(uint category,
            string paramsSubString = null,
            string brand = null,
            uint byPage = 10,
            uint page = 1,
            bool ignoreCase = true)
        {
            var take = byPage;
            var skip = page > 1 ? (page - 1) * byPage : 0;

            return await _productsRepository.GetProductsByCategory(
                category: category,
                paramsSubString: paramsSubString,
                brandSubStr: brand,
                take: take,
                skipCount: skip,
                ignoreCase: ignoreCase);
        }

        public async Task<Result> UpdateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(ResultMessager.PRODUCT_RARAM_NAME);

            var unValidatedProductResult = UnValidatedProductResult(product);
            if (unValidatedProductResult != null)
                return unValidatedProductResult;


            return await _productsRepository.UpdateProduct(product);
        }

        /// <summary>
        /// Валидация полей товара Product
        /// </summary>
        /// <param name="product"> Product - товар </param>
        /// <returns> Result - при ошибке | null, если проверка пройдена </returns>
        private Result UnValidatedProductResult(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
                return new Result(ResultMessager.NAME_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(product.ArticleNumber))
                return new Result(ResultMessager.ARTICLENUMBER_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(product.Brand))
                return new Result(ResultMessager.BRAND_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(product.Params))
                return new Result(ResultMessager.PARAMS_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(product.Url))
                return new Result(ResultMessager.URL_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(product.ImageUrl))
                return new Result(ResultMessager.IMAGEURL_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (product.GoodsGroups == null || !product.GoodsGroups.Any())
                return new Result(ResultMessager.GOODGROUPS_SHOULD_NOT_BE_NULL_OR_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(product.Dimensions))
                return new Result(ResultMessager.DIMENSIONS_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            if (product.PriceId != null)
            {
                if (product.PricePerUnit == null || product.PricePerUnit <= 0)
                    return new Result(ResultMessager.PRICEPERUNIT_SHOULD_BE_NULL_OR_POSITIVE, System.Net.HttpStatusCode.BadRequest);

                return null;
            }
            //product.PriceId == null
            if (product.PricePerUnit != null || product.PricePerUnit <= 0)
                return new Result(ResultMessager.PRICEPERUNIT_SHOULD_BE_NULL_OR_POSITIVE, System.Net.HttpStatusCode.BadRequest);

            return null;
        }
    }
}
