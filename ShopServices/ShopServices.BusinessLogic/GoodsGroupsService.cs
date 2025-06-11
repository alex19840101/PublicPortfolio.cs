using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Models;
using ShopServices.Core.Repositories;
using ShopServices.Core.Services;

namespace ShopServices.BusinessLogic
{
    public class GoodsGroupsService : IGoodsGroupsService
    {
        private readonly ICategoriesRepository _categoriesRepository;

        public GoodsGroupsService(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }
        public async Task<Result> AddCategory(Category newCategory)
        {
            if (newCategory == null)
                throw new ArgumentNullException(ResultMessager.NEWCATEGORY_RARAM_NAME);

            if (string.IsNullOrWhiteSpace(newCategory.Name))
                return new Result(ResultMessager.NAME_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            var existingCategory = await _categoriesRepository.GetCategoryByName(newCategory.Name);

            if (existingCategory != null)
            {
                if (!existingCategory.IsEqualIgnoreIdAndDt(newCategory))
                    return new Result(ResultMessager.CONFLICT, System.Net.HttpStatusCode.Conflict);

                return new Result(ResultMessager.ALREADY_EXISTS, System.Net.HttpStatusCode.Created, id: existingCategory.Id);
            }

            var createResult = await _categoriesRepository.Create(newCategory);

            return createResult;
        }

        public async Task<Result> ArchiveCategory(uint id)
        {
            return await _categoriesRepository.ArchiveCategoryById(id);
        }

        public async Task<Result> DeleteCategory(uint id)
        {
            return await _categoriesRepository.DeleteCategoryById(id);
        }

        public async Task<IEnumerable<Category>> GetCategories(
            string nameSubString = null,
            string brand = null,
            uint byPage = 10,
            uint page = 1,
            bool ignoreCase = true)
        {
            var take = byPage;
            var skip = page > 1 ? (page - 1) * byPage : 0;

            return await _categoriesRepository.GetCategories(
                nameSubString,
                brand,
                take,
                skip);
        }

        public async Task<Category> GetCategoryById(uint id)
        {
            return await _categoriesRepository.GetCategoryById(id);
        }

        public async Task<Category> GetCategoryByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return await _categoriesRepository.GetCategoryByName(name);
        }

        public async Task<Result> UpdateCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(ResultMessager.CATEGORY_RARAM_NAME);

            if (string.IsNullOrWhiteSpace(category.Name))
                return new Result(ResultMessager.NAME_SHOULD_NOT_BE_EMPTY, System.Net.HttpStatusCode.BadRequest);

            return await _categoriesRepository.UpdateCategory(category);
        }
    }
}
