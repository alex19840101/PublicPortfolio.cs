using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;

namespace ShopServices.Core.Services
{
    /// <summary> Интерфейс для работы с группами (категориями) товаров </summary>
    public interface IGoodsGroupsService
    {
        public Task<Result> AddCategory(Category product);
        public Task<Category> GetCategoryById(uint id);
        public Task<IEnumerable<Category>> GetCategoryByName(string nameSubString);
        public Task<IEnumerable<Category>> GetCategories(
            string nameSubString,
            string brand = null,
            uint byPage = 10,
            uint page = 1);
        public Task<Result> UpdateCategory(Category product);
        public Task<Result> DeleteCategory(uint id);
    }
}
