using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warehub.model;

namespace warehub.db
{
    public class ProductRepository
    {
        internal GenericResponseDTO<Product> Add(Product product)
        {
            throw new NotImplementedException();
        }

        internal GenericResponseDTO<Product> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        internal GenericResponseDTO<IEnumerable<Product>> GetAll()
        {
            throw new NotImplementedException();
            // Your logic to get data here
        }

        internal GenericResponseDTO<Product> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        internal GenericResponseDTO<Product> Update(object existingProduct)
        {
            throw new NotImplementedException();
        }
    }
}
