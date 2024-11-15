using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using warehub.model;
using warehub.services.interfaces;

namespace warehub.db
{
    public class ProductRepository
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
                { "id", product.Id }
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
            List<Product> listOfProducts = ConvertToProducts(products);
            var returnObject = new GenericResponseDTO<List<Product>>(listOfProducts)
            {
                IsSuccess = status
            };
            return returnObject;
        }

        public GenericResponseDTO<Product> GetById(Guid id)
        {
            var (status, products) = _cRUDService.Read("products", new Dictionary<string, object> { { "id", id } });
            List<Product> listOfProducts = ConvertToProducts(products);
            
            Product product = listOfProducts.FirstOrDefault(p => p.Id == id);
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
                { "price", product.Price }
            };
            bool status = _cRUDService.Update("products", updateParams, "id", product.Id);

            var returnObject = new GenericResponseDTO<Product>(product)
            {
                IsSuccess = status
            };
            return returnObject;
        }

        // DELETE when factory is implementet
        public List<Product> ConvertToProducts(List<Dictionary<string, object>> products)
        {
            var productList = new List<Product>();

            foreach (var productDict in products)
            {
                Guid id;
                if (productDict.ContainsKey("id") && productDict["id"] is Guid)
                {
                    id = (Guid)productDict["id"];
                }
                else
                {
                    continue;
                }

                string name;
                if (productDict.ContainsKey("name") && productDict["name"] is string)
                {
                    name = (string)productDict["name"];
                }
                else
                {
                    continue;
                }

                int price;
                if (productDict.ContainsKey("price") && productDict["price"] is int)
                {
                    price = (int)productDict["price"];
                }
                else
                {
                    continue;
                }

                // Create new Product instance with parsed values
                var product = ProductFactory.CreateProduct(id, name, price);
                productList.Add(product);
            }

            return productList;
        }
    }
}
