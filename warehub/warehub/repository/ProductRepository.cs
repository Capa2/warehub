using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using warehub.db;
using warehub.model;
using warehub.services.interfaces;

namespace warehub.repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly CRUDService _cRUDService;

        public ProductRepository()
        {
            MySqlConnection connection = DbConnection.GetConnection();
            _cRUDService = new CRUDService(connection);
        }

        public GenericResponseDTO<Product> Add(Product product)
        {
            var parameters = new Dictionary<string, object>
            {
                { "name", product.Name },
                { "price", product.Price },
                { "id", product.Id },
                { "amount", product.Amount }
            };
            bool status = _cRUDService.Create("products", parameters);
            var returnObject = new GenericResponseDTO<Product>(product)
            {
                IsSuccess = status
            };
            return returnObject;
        }

        public GenericResponseDTO<Guid> Delete(Guid id)
        {
            bool status = _cRUDService.Delete("products", "id", id);
            var returnObject = new GenericResponseDTO<Guid>(id)
            {
                IsSuccess = status
            };
            return returnObject;
        }

        public GenericResponseDTO<List<Product>> GetAll()
        {
            var (status, products) = _cRUDService.Read("products", new Dictionary<string, object>());
            List<Product> listOfProducts = ObjectMapper.MapDictToProducts(products);
            var returnObject = new GenericResponseDTO<List<Product>>(listOfProducts)
            {
                IsSuccess = status
            };
            return returnObject;
        }

        public GenericResponseDTO<Product> GetById(Guid id)
        {
            var (status, products) = _cRUDService.Read("products", new Dictionary<string, object> { { "id", id } });
            List<Product> listOfProducts = ObjectMapper.MapDictToProducts(products);

            var product = listOfProducts.FirstOrDefault(p => p.Id == id);
            var returnObject = new GenericResponseDTO<Product>(product)
            {
                IsSuccess = status
            };
            return returnObject;
        }

        public GenericResponseDTO<Product> Update(Product product)
        {
            var updateParams = new Dictionary<string, object>
            {
                { "name", product.Name },
                { "price", product.Price },
                { "amount", product.Amount }
            };
            bool status = _cRUDService.Update("products", updateParams, "id", product.Id);

            var returnObject = new GenericResponseDTO<Product>(product)
            {
                IsSuccess = status
            };
            return returnObject;
        }
    }
}
