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
        public IProduct? GetProductById(Guid id);
        public List<IProduct>? GetAllProducts();
        public bool AddProduct(IProduct product);
        public bool UpdateProduct(IProduct product);
        public bool DeleteProduct(Guid id);
    }
}
