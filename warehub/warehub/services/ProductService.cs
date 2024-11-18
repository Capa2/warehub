using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warehub.model;
using warehub.repository;
using warehub.services.interfaces;

namespace warehub.services
{
    public class ProductService(ProductRepository productRepository) : IProductSerivce
    {

        private readonly ProductRepository _productRepository = productRepository;

        public List<Product>? GetAllProducts()
        {
            GenericResponseDTO<List<Product>> responseObject = _productRepository.GetAll();
            if (responseObject.IsSuccess)
            {
                return responseObject.Data;
            }
            return null;
        }

        public Product? GetProductById(Guid id)
        {
            GenericResponseDTO<Product> response = _productRepository.GetById(id);
            if (response.IsSuccess)
            {
                return response.Data;
            }
            return null;
        }

        public bool AddProduct(Product product)
        {
            GenericResponseDTO<Product> response = _productRepository.Add(product);
            if (response.IsSuccess)
            {
                return true;
            }
            return false;
        }

        public bool UpdateProduct(Product product)
        {
            GenericResponseDTO<Product> getResponse = _productRepository.GetById(product.Id);
            if (!getResponse.IsSuccess)
            {
                return false;
            }
            GenericResponseDTO<Product> response = _productRepository.Update(product);
            if (response.IsSuccess)
            {
                return true;
            }
            return false;
        }

        public bool DeleteProduct(Guid id)
        {
            GenericResponseDTO<Product> getResponse = _productRepository.GetById(id);
            if (!getResponse.IsSuccess)
            {
                return false;
            }
            GenericResponseDTO<Guid> response = _productRepository.Delete(id);

            if (response.IsSuccess)
            {
                return true;
            }
            return false;
        }
    }

}
