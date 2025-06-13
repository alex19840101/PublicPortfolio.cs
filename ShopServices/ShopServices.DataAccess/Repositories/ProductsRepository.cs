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
    public class ProductsRepository : IProductsRepository
    {
        private readonly ShopServicesDbContext _dbContext;

        public ProductsRepository(ShopServicesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Create(Product newProduct)
        {
            ArgumentNullException.ThrowIfNull(newProduct);

            var newProductEntity = new Entities.Product
            (
                id: newProduct.Id,
                articleNumber: newProduct.ArticleNumber,
                brand: newProduct.Brand,
                name: newProduct.Name,
                parameters: newProduct.Params,
                url: newProduct.Url,
                imageUrl: newProduct.ImageUrl,
                goodsGroups: newProduct.GoodsGroups,
                archieved: newProduct.Archieved,
                massInGrams: newProduct.MassInGrams,
                dimensions: newProduct.Dimensions,
                created: DateTime.Now.ToUniversalTime(),
                updated: null
            );

            await _dbContext.Products.AddAsync(newProductEntity);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(newProductEntity).GetDatabaseValuesAsync(); //получение сгенерированного БД id
            return new Result
            {
                Id = newProductEntity.Id,
                StatusCode = HttpStatusCode.Created,
                Message = ResultMessager.OK
            };
        }

        public async Task<Product?> GetProductById(uint id)
        {
            var productEntity = await GetProductEntity(id, asNoTracking: true);
            if (productEntity is null)
                return null;

            return GetCoreProduct(productEntity);
        }

        public async Task<Product?> GetProductByName(string name)
        {
            var productEntity = await GetProductEntity(name);
            if (productEntity is null)
                return null;

            return GetCoreProduct(productEntity);
        }

        public async Task<IEnumerable<Product>> GetProductsByArticle(
            string articleSubString,
            string brandSubStr,
            uint take,
            uint skipCount,
            bool ignoreCase = true)
        {
            var limitCount = take > 100 ? 100 : take;
            List<Entities.Product> entityProductsLst;

            if (string.IsNullOrWhiteSpace(brandSubStr) && string.IsNullOrWhiteSpace(articleSubString))
            {
                entityProductsLst = await _dbContext.Products
                            .AsNoTracking()
                            .Skip((int)skipCount).Take((int)limitCount).ToListAsync();

                if (entityProductsLst.Count == 0)
                    return [];

                return entityProductsLst.Select(p => GetCoreProduct(p));
            }
            Expression<Func<Entities.Product, bool>> expressionWhereName = ignoreCase ?
                p => EF.Functions.Like(p.ArticleNumber.ToLower(), $"%{articleSubString.ToLower()}%") :
                p => p.ArticleNumber.Contains(articleSubString);

            if (string.IsNullOrWhiteSpace(brandSubStr))
            {
                entityProductsLst = await _dbContext.Products
                        .AsNoTracking()
                        .Where(expressionWhereName).Skip((int)skipCount).Take((int)limitCount).ToListAsync();

                if (entityProductsLst.Count == 0)
                    return [];

                return entityProductsLst.Select(c => GetCoreProduct(c));
            }

            //brandSubStr задан
            Expression<Func<Entities.Product, bool>> expressionWhereBrand = ignoreCase ?
                p => EF.Functions.Like(p.Brand!.ToLower(), $"%{brandSubStr.ToLower()}%") :
                p => p.Brand!.Contains(brandSubStr);

            entityProductsLst = string.IsNullOrWhiteSpace(articleSubString) ?
                await _dbContext.Products
                        .AsNoTracking()
                        .Where(expressionWhereBrand).Skip((int)skipCount).Take((int)limitCount).ToListAsync() :
                await _dbContext.Products
                        .AsNoTracking()
                        .Where(expressionWhereBrand)
                        .Where(expressionWhereName).Skip((int)skipCount).Take((int)limitCount)
                        .ToListAsync();

            if (entityProductsLst.Count == 0)
                return [];

            return entityProductsLst.Select(p => GetCoreProduct(p));
        }

        public async Task<IEnumerable<Product>> GetProducts(
            string nameSubString,
            string brandSubStr,
            uint take,
            uint skipCount,
            bool ignoreCase = true)
        {
            var limitCount = take > 100 ? 100 : take;
            List<Entities.Product> entityProductsLst;

            if (string.IsNullOrWhiteSpace(brandSubStr) && string.IsNullOrWhiteSpace(nameSubString))
            {
                entityProductsLst = await _dbContext.Products
                            .AsNoTracking()
                            .Skip((int)skipCount).Take((int)limitCount).ToListAsync();

                if (entityProductsLst.Count == 0)
                    return [];

                return entityProductsLst.Select(p => GetCoreProduct(p));
            }
            Expression<Func<Entities.Product, bool>> expressionWhereName = ignoreCase ?
                p => EF.Functions.Like(p.Name.ToLower(), $"%{nameSubString.ToLower()}%") :
                p => p.Name.Contains(nameSubString);

            if (string.IsNullOrWhiteSpace(brandSubStr))
            {
                entityProductsLst = await _dbContext.Products
                        .AsNoTracking()
                        .Where(expressionWhereName).Skip((int)skipCount).Take((int)limitCount).ToListAsync();

                if (entityProductsLst.Count == 0)
                    return [];

                return entityProductsLst.Select(c => GetCoreProduct(c));
            }

            //brandSubStr задан
            Expression<Func<Entities.Product, bool>> expressionWhereBrand = ignoreCase ?
                p => EF.Functions.Like(p.Brand!.ToLower(), $"%{brandSubStr.ToLower()}%") :
                p => p.Brand!.Contains(brandSubStr);

            entityProductsLst = string.IsNullOrWhiteSpace(nameSubString) ?
                await _dbContext.Products
                        .AsNoTracking()
                        .Where(expressionWhereBrand).Skip((int)skipCount).Take((int)limitCount).ToListAsync() :
                await _dbContext.Products
                        .AsNoTracking()
                        .Where(expressionWhereBrand)
                        .Where(expressionWhereName).Skip((int)skipCount).Take((int)limitCount)
                        .ToListAsync();

            if (entityProductsLst.Count == 0)
                return [];

            return entityProductsLst.Select(p => GetCoreProduct(p));
        }

        public async Task<Result> UpdateProduct(Product upd)
        {
            ArgumentNullException.ThrowIfNull(upd);

            var productEntity = await _dbContext.Products
                .SingleOrDefaultAsync(p => p.Id == upd.Id);

            if (productEntity is null)
                return new Result(ResultMessager.NOT_FOUND, HttpStatusCode.NotFound);

            if (!string.Equals(upd.Name, productEntity.Name)) productEntity.UpdateName(upd.Name);
            if (!string.Equals(upd.Brand, productEntity.Brand)) productEntity.UpdateBrand(upd.Brand);
            if (!string.Equals(upd.Params, productEntity.Parameters)) productEntity.UpdateParameters(upd.Params);
            if (!string.Equals(upd.Url, productEntity.Url)) productEntity.UpdateUrl( upd.Url);
            if (!string.Equals(upd.ImageUrl, productEntity.ImageUrl)) productEntity.UpdateImageUrl(upd.ImageUrl);
            if (!string.Equals(upd.Archieved, productEntity.Archieved)) productEntity.UpdateArchived(upd.Archieved);
            
            if (!string.Equals(upd.Dimensions, productEntity.Dimensions)) productEntity.UpdateDimensions(upd.Dimensions);
            if (upd.MassInGrams != productEntity.MassInGrams) productEntity.UpdateMassInGrams(upd.MassInGrams);

            if (upd.PriceId != productEntity.PriceId) productEntity.UpdatePriceId(upd.PriceId);
            if (upd.PricePerUnit != productEntity.PricePerUnit) productEntity.UpdatePricePerUnit(upd.PricePerUnit);

            productEntity.UpdateGoodsGroups(upd.GoodsGroups);

            if (_dbContext.ChangeTracker.HasChanges())
            {
                productEntity.UpdateUpdatedDt(DateTime.Now.ToUniversalTime());
                await _dbContext.SaveChangesAsync();
                return new Result(ResultMessager.PRODUCT_UPDATED, HttpStatusCode.OK);
            }

            return new Result(ResultMessager.PRODUCT_IS_ACTUAL, HttpStatusCode.OK);
        }

        public async Task<Result> ArchiveProductById(uint id)
        {
            var entityProduct = await _dbContext.Products
                .SingleOrDefaultAsync(p => p.Id == id);

            if (entityProduct is null)
                return new Result(ResultMessager.NOT_FOUND, HttpStatusCode.NotFound);

            if (!entityProduct.Archieved)
            {
                entityProduct.UpdateArchived(true);

                await _dbContext.SaveChangesAsync();
            }

            return new Result(ResultMessager.OK, HttpStatusCode.OK);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(
            uint category,
            string paramsSubString,
            string brandSubStr,
            uint take,
            uint skipCount,
            bool ignoreCase = true)
        {
            var limitCount = take > 100 ? 100 : take;
            List<Entities.Product> entityProductsLst;
            Expression<Func<Entities.Product, bool>> expressionWhereGoodsGroupsHasCategory = p => p.GoodsGroups != null && p.GoodsGroups.Contains(category);
            if (string.IsNullOrWhiteSpace(brandSubStr) && string.IsNullOrWhiteSpace(paramsSubString))
            {
                entityProductsLst = await _dbContext.Products
                            .AsNoTracking().Where(expressionWhereGoodsGroupsHasCategory)
                            .Skip((int)skipCount).Take((int)limitCount).ToListAsync();  //TODO: fix Npgsql.PostgresException: "42883: оператор не существует: integer[] @> bigint[]

                if (entityProductsLst.Count == 0)
                    return [];

                return entityProductsLst.Select(p => GetCoreProduct(p));
            }
            Expression<Func<Entities.Product, bool>> expressionWhereName = ignoreCase ?
                p => EF.Functions.Like(p.Parameters.ToLower(), $"%{paramsSubString.ToLower()}%") :
                p => p.Parameters.Contains(paramsSubString);

            if (string.IsNullOrWhiteSpace(brandSubStr))
            {
                entityProductsLst = await _dbContext.Products
                        .AsNoTracking()
                        .Where(expressionWhereName).Skip((int)skipCount).Take((int)limitCount).ToListAsync();

                if (entityProductsLst.Count == 0)
                    return [];

                return entityProductsLst.Select(c => GetCoreProduct(c));
            }

            //brandSubStr задан
            Expression<Func<Entities.Product, bool>> expressionWhereBrand = ignoreCase ?
                p => EF.Functions.Like(p.Brand!.ToLower(), $"%{brandSubStr.ToLower()}%") :
                p => p.Brand!.Contains(brandSubStr);

            entityProductsLst = string.IsNullOrWhiteSpace(paramsSubString) ?
                await _dbContext.Products
                        .AsNoTracking()
                        .Where(expressionWhereGoodsGroupsHasCategory)
                        .Where(expressionWhereBrand).Skip((int)skipCount).Take((int)limitCount).ToListAsync() :
                await _dbContext.Products
                        .AsNoTracking()
                        .Where(expressionWhereGoodsGroupsHasCategory)
                        .Where(expressionWhereBrand)
                        .Where(expressionWhereName).Skip((int)skipCount).Take((int)limitCount)
                        .ToListAsync();

            if (entityProductsLst.Count == 0)
                return [];

            return entityProductsLst.Select(p => GetCoreProduct(p));
        }

        private static Product GetCoreProduct(Entities.Product productEntity) =>
            new Product(
                id: productEntity.Id,
                articleNumber: productEntity.ArticleNumber,
                brand: productEntity.Brand,
                name: productEntity.Name,
                parameters: productEntity.Parameters,
                url: productEntity.Url,
                imageUrl: productEntity.ImageUrl,
                goodsGroups: productEntity.GoodsGroups,
                archieved: productEntity.Archieved,
                massInGrams: productEntity.MassInGrams,
                dimensions: productEntity.Dimensions,
                priceId: productEntity.PriceId,
                pricePerUnit: productEntity.PricePerUnit,
                created: productEntity.Created.ToLocalTime(),
                updated: productEntity.Updated?.ToLocalTime());


        private async Task<Entities.Product?> GetProductEntity(string name)
        {
            var query = _dbContext.Products.AsNoTracking().Where(p => p.Name.Equals(name));
            var productEntity = await query.SingleOrDefaultAsync();

            return productEntity;
        }

        private async Task<Entities.Product?> GetProductEntity(uint id, bool asNoTracking)
        {
            var query = asNoTracking ?
                _dbContext.Products.AsNoTracking().Where(p => p.Id == id) :
                _dbContext.Products.Where(p => p.Id == id);

            var productEntity = await query.SingleOrDefaultAsync();

            return productEntity;
        }
    }
}
