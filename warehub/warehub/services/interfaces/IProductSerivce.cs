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
        public Product GetProductById(Guid id);
        public IEnumerable<Product> GetAllProducts();
        public void AddProduct(Product product);
        public void UpdateProduct(Product product);
        public void DeleteProduct(Guid id);
    }
}
