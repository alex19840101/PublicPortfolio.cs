using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;

namespace ShopServices.Core.Repositories
{
    public interface IProductsRepository
    {
        public Task<Result> Create(Product newProduct);
        public Task<Product> GetProductById(uint id);
        public Task<Product> GetProductByName(string name);
        
        Task<IEnumerable<Product>> GetProducts(
            string nameSubString,
            string brandSubStr,
            uint take,
            uint skipCount,
            bool ignoreCase = true);
        Task<IEnumerable<Product>> GetProductsByArticle(
            string articleSubString,
            string brandSubStr,
            uint take,
            uint skipCount,
            bool ignoreCase = true);
        Task<IEnumerable<Product>> GetProductsByCategory(
            uint category,
            string paramsSubString,
            string brandSubStr,
            uint take,
            uint skipCount,
            bool ignoreCase = true);
        public Task<Result> UpdateProduct(Product product);
        public Task<Result> ArchiveProductById(uint id);

    }
}
