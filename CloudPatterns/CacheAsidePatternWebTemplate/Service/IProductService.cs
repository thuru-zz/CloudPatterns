using CacheAsidePatternWebTemplate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheAsidePatternWebTemplate.Service
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync();
        Task<Product> CreateProudctAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
    }
}
