using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warehub.model;

namespace warehub.services.interfaces
{
    public interface IProductSerivce
    {
        public Product? GetProductById(Guid id);
        public IEnumerable<Product>? GetAllProducts();
        public bool AddProduct(Product product);
        public bool UpdateProduct(Product product);
        public bool DeleteProduct(Guid id);
    }
}
