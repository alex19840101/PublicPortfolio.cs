﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;

namespace ShopServices.Core.Services
{
    public interface IGoodsService
    {
        public Task<Result> AddProduct(Product product);
        public Task<Product> GetProductById(uint id);
        public Task<IEnumerable<Product>> GetProductsByArticle(
            string articleSubString,
            string brand = null,
            uint byPage = 10,
            uint page = 1,
            bool ignoreCase = true);
        public Task<IEnumerable<Product>> GetProducts(
            string nameSubString,
            string brand = null,
            uint byPage = 10,
            uint page = 1,
            bool ignoreCase = true);
        public Task<Result> UpdateProduct(Product product);
        public Task<Result> ArchiveProduct(uint id);
        public Task<IEnumerable<Product>> GetProductsByCategory(
            uint category,
            string paramsSubString = null,
            string brand = null,
            uint byPage = 10,
            uint page = 1,
            bool ignoreCase = true);
    }
}
