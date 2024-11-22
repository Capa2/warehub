using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warehub.model;
using warehub.model.interfaces;

namespace warehub.services.interfaces
{
    public interface IProductService
    {
        Task<bool> AddProduct(IProduct product);
        Task<bool> DeleteProduct(Guid id);
        Task<List<IProduct>?> GetAllProducts();
        Task<IProduct?> GetProductByIdAsync(Guid id);
        Task<bool> UpdateProduct(IProduct product);
    }
}
