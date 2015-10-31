using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CacheAsidePatternWebTemplate.Models;
using CacheAsidePatternWebTemplate.Cache;

namespace CacheAsidePatternWebTemplate.Service
{
    public class ProductService : IProductService
    {
        private readonly CacheProvider<Product> _cacheProvider;

        public ProductService(CacheProvider<Product> provider)
        {
            _cacheProvider = provider;
        }

        public async Task<Product> CreateProudctAsync(Product product)
        {
            // save product in the data store

            await _cacheProvider.SetCollectionAsync(product.CacheKey, new List<Product>() { product });
            return product;
        }

        public Task<Product> GetProductByIdAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> UpdateProductAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}