using System.Collections.Generic;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;
using ShopServices.Core.Services;

namespace ShopServices.BusinessLogic
{
    public class GoodsService : IGoodsService
    {
        public Task<Result> AddProduct(Product product)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result> DeleteProduct(uint id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Product> GetProductById(uint id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetProducts(string nameSubString, string brand = null, uint byPage = 10, uint page = 1)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetProductsByArticle(string articleSubString)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result> UpdateProduct(Product product)
        {
            throw new System.NotImplementedException();
        }
    }
}
