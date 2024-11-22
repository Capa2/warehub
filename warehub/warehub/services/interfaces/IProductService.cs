using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warehub.model;

namespace warehub.services.interfaces
{
    public interface IProductService
    {
        public Task<Product?> GetProductByIdAsync(Guid id);
        public Task<List<Product>?> GetAllProducts();
        public Task<bool> AddProduct(Product product);
        public Task<bool> UpdateProduct(Product product);
        public Task<bool> DeleteProduct(Guid id);
    }
}
