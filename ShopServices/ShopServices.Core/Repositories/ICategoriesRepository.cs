using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;

namespace ShopServices.Core.Repositories
{
    public interface ICategoriesRepository
    {
        public Task<Result> Create(Category newCategory);
        public Task<Category> GetCategoryById(uint id);
        public Task<Category> GetCategoryByName(string name);
        public Task<IEnumerable<Category>> GetCategories(
            string nameSubString,
            string brandSubStr,
            uint take,
            uint skipCount,
            bool ignoreCase = true);
        public Task<Result> UpdateCategory(Category category);
        public Task<Result> ArchiveCategoryById(uint id);
        public Task<Result> DeleteCategoryById(uint id);
    }
}
