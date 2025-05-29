using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;
using ShopServices.Core.Services;

namespace ShopServices.BusinessLogic
{
    public class GoodsGroupsService : IGoodsGroupsService
    {
        public async Task<Result> AddCategory(Category product)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> DeleteCategory(uint id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Category>> GetCategories(
            string nameSubString,
            string brand = null,
            uint byPage = 10,
            uint page = 1)
        {
            throw new NotImplementedException();
        }

        public async Task<Category> GetCategoryById(uint id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Category>> GetCategoryByName(string nameSubString)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> UpdateCategory(Category product)
        {
            throw new NotImplementedException();
        }
    }
}
