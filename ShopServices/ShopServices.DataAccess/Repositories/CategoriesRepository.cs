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
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly ShopServicesDbContext _dbContext;

        public CategoriesRepository(ShopServicesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Create(Category newCategory)
        {
            ArgumentNullException.ThrowIfNull(newCategory);

            var newCategoryEntity = new Entities.Category
            {
                Name = newCategory.Name,
                Brand = newCategory.Brand,
                Params = newCategory.Params,
                Url = newCategory.Url,
                ImageUrl = newCategory.ImageUrl,
                Archieved = newCategory.Archived,
                Created = DateTime.Now.ToUniversalTime(),
                Updated = newCategory.Updated?.ToUniversalTime(),
            };

            await _dbContext.Categories.AddAsync(newCategoryEntity);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(newCategoryEntity).GetDatabaseValuesAsync(); //получение сгенерированного БД id
            return new Result
            {
                Id = newCategoryEntity.Id,
                StatusCode = HttpStatusCode.Created,
                Message = ResultMessager.OK
            };
        }

        public async Task<Category?> GetCategoryByName(string name)
        {
            var categoryEntity = await GetCategoryEntity(name);
            if (categoryEntity is null)
                return null;

            return GetCoreCategory(categoryEntity);
        }

        public async Task<Category?> GetCategoryById(uint id)
        {
            var categoryEntity = await GetCategoryEntity(id, asNoTracking: true);
            if (categoryEntity is null)
                return null;

            return GetCoreCategory(categoryEntity);
        }

        public async Task<IEnumerable<Category>> GetCategories(
            string nameSubString,
            string brandSubStr,
            uint take,
            uint skipCount,
            bool ignoreCase = true)
        {
            var limitCount = take > 100 ? 100 : take;
            List<Entities.Category> entityCategorysLst;

            if (string.IsNullOrWhiteSpace(brandSubStr) && string.IsNullOrWhiteSpace(nameSubString))
            {
                entityCategorysLst = await _dbContext.Categories
                            .AsNoTracking()
                            .Skip((int)skipCount).Take((int)limitCount).ToListAsync();

                if (entityCategorysLst.Count == 0)
                    return [];

                return entityCategorysLst.Select(c => GetCoreCategory(c));
            }
            Expression<Func<Entities.Category, bool>> expressionWhereName = ignoreCase ?
                c => EF.Functions.Like(c.Name.ToLower(), $"%{nameSubString.ToLower()}%") :
                c => c.Name.Contains(nameSubString);

            if (string.IsNullOrWhiteSpace(brandSubStr))
            {
                entityCategorysLst = await _dbContext.Categories
                        .AsNoTracking()
                        .Where(expressionWhereName).Skip((int)skipCount).Take((int)limitCount).ToListAsync();

                if (entityCategorysLst.Count == 0)
                    return [];

                return entityCategorysLst.Select(c => GetCoreCategory(c));
            }

            //brandSubStr задан
            Expression<Func<Entities.Category, bool>> expressionWhereBrand = ignoreCase ?
                c => EF.Functions.Like(c.Brand!.ToLower(), $"%{brandSubStr.ToLower()}%") :
                c => c.Brand!.Contains(brandSubStr);

            entityCategorysLst = string.IsNullOrWhiteSpace(nameSubString) ?
                await _dbContext.Categories
                        .AsNoTracking()
                        .Where(expressionWhereBrand).Skip((int)skipCount).Take((int)limitCount).ToListAsync() :
                await _dbContext.Categories
                        .AsNoTracking()
                        .Where(expressionWhereBrand)
                        .Where(expressionWhereName).Skip((int)skipCount).Take((int)limitCount)
                        .ToListAsync();

            if (entityCategorysLst.Count == 0)
                return [];

            return entityCategorysLst.Select(c => GetCoreCategory(c));
        }

        public async Task<Result> UpdateCategory(Category upd)
        {
            ArgumentNullException.ThrowIfNull(upd);

            var categoryEntity = await _dbContext.Categories
                .SingleOrDefaultAsync(c => c.Id == upd.Id);

            if (categoryEntity is null)
                return new Result(ResultMessager.NOT_FOUND, HttpStatusCode.NotFound);

            if (!string.Equals(upd.Name, categoryEntity.Name)) categoryEntity.Name = upd.Name;
            if (!string.Equals(upd.Brand, categoryEntity.Brand)) categoryEntity.Brand = upd.Brand;
            if (!string.Equals(upd.Params, categoryEntity.Params)) categoryEntity.Params = upd.Params;
            if (!string.Equals(upd.Url, categoryEntity.Url)) categoryEntity.Url = upd.Url;
            if (!string.Equals(upd.ImageUrl, categoryEntity.ImageUrl)) categoryEntity.ImageUrl = upd.ImageUrl;
            if (!string.Equals(upd.Archived, categoryEntity.Archieved)) categoryEntity.Archieved=upd.Archived;

            if (_dbContext.ChangeTracker.HasChanges())
            {
                categoryEntity.Updated = DateTime.Now.ToUniversalTime();
                await _dbContext.SaveChangesAsync();
                return new Result(ResultMessager.CATEGORY_UPDATED, HttpStatusCode.OK);
            }

            return new Result(ResultMessager.CATEGORY_IS_ACTUAL, HttpStatusCode.OK);
        }
        public async Task<Result> ArchiveCategoryById(uint id)
        {
            var entityCategory = await _dbContext.Categories
                .SingleOrDefaultAsync(c => c.Id == id);

            if (entityCategory is null)
                return new Result(ResultMessager.NOT_FOUND, HttpStatusCode.NotFound);

            if (!entityCategory.Archieved)
            {
                entityCategory.Archieved = true;

                await _dbContext.SaveChangesAsync();
            }

            return new Result(ResultMessager.OK, HttpStatusCode.OK);
        }

        public async Task<Result> DeleteCategoryById(uint id)
        {
            var entityCategory = await _dbContext.Categories
                .SingleOrDefaultAsync(c => c.Id == id);

            if (entityCategory is null)
                return new Result(ResultMessager.NOT_FOUND, HttpStatusCode.NotFound);

            _dbContext.Categories.Remove(entityCategory);
            await _dbContext.SaveChangesAsync();

            return new Result(ResultMessager.OK, HttpStatusCode.OK);
        }

        private async Task<Entities.Category?> GetCategoryEntity(string name)
        {
            var query = _dbContext.Categories.AsNoTracking().Where(c => c.Name.Equals(name));
            var categoryEntity = await query.SingleOrDefaultAsync();

            return categoryEntity;
        }

        private async Task<Entities.Category?> GetCategoryEntity(uint id, bool asNoTracking)
        {
            var query = asNoTracking ?
                _dbContext.Categories.AsNoTracking().Where(c => c.Id == id) :
                _dbContext.Categories.Where(c => c.Id == id);

            var categoryEntity = await query.SingleOrDefaultAsync();

            return categoryEntity;
        }

        private static Category GetCoreCategory(Entities.Category categoryEntity) =>
            new Category(
                id: categoryEntity.Id,
                name: categoryEntity.Name,
                brand: categoryEntity.Brand,
                url: categoryEntity.Url,
                imageUrl: categoryEntity.ImageUrl,
                parameters: categoryEntity.Params,
                archived: categoryEntity.Archieved,
                created: categoryEntity.Created.ToLocalTime(),
                updated: categoryEntity.Updated?.ToLocalTime());
    }
}
