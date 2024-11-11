using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warehub.db;
using warehub.model;
using warehub.services.interfaces;

namespace warehub.services
{
    public class ProductService: IProductSerivce
    {

        private readonly ProductRepository _productRepository;

        // Constructor injection to get an instance of IProductRepository
        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IEnumerable<Product>? GetAllProducts()
        {
            
            GenericResponseDTO<IEnumerable<Product>> responseObject = _productRepository.GetAll();
            if (responseObject.IsSuccess)
            {
                return responseObject.Data;
            }
            return null;
        }

        public Product GetProductById(Guid id)
        {   
            Product product = null;
            productDTO = _productRepository.GetById(id);
            if (product == null)
            {
                throw new Exception("Product not found");
            }?? throw new Exception("Product not found");
        }

        // Method to add a new product
        public void AddProduct(Product product)
        {
            _productRepository.Add(product);
        }

        // Method to update an existing product
        public void UpdateProduct(Product product)
        {
            var existingProduct = _productRepository.GetById(product.Id);
            if (existingProduct == null)
            {
                throw new Exception("Product not found");
            }
            _productRepository.Update(existingProduct);
        }

        // Method to delete a product by ID
        public void DeleteProduct(Guid id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            _productRepository.Delete(id);
        }
    }

}
