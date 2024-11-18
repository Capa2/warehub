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

        // DELETE when factory is implementet
        public List<Product> ConvertToProducts(List<Dictionary<string, object>> products)
        {
            var productList = new List<Product>();

            foreach (var productDict in products)
            {
                // Log the contents of productDict for debugging
                Console.WriteLine("Processing product dictionary:");
                foreach (var kvp in productDict)
                {
                    Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}, Type: {kvp.Value?.GetType()}");
                }

                // Parse the 'id' field
                if (!productDict.ContainsKey("id") || productDict["id"] is not Guid id)
                {
                    Console.WriteLine("Skipping product due to invalid or missing 'id'.");
                    continue;
                }

                // Parse the 'name' field
                if (!productDict.ContainsKey("name") || productDict["name"] is not string name)
                {
                    Console.WriteLine("Skipping product due to invalid or missing 'name'.");
                    continue;
                }

                // Parse the 'price' field
                if (!productDict.ContainsKey("price") || productDict["price"] is not decimal price)
                {
                    Console.WriteLine("Skipping product due to invalid or missing 'price'.");
                    continue;
                }
                int amount;
                if (productDict.ContainsKey("Amount") && productDict["Amount"] is int)
                {
                    amount = (int)productDict["Amount"];
                }
                else
                {
                    continue;
                }

                // Create a new Product instance
                var product = ProductFactory.CreateProduct(id, name, price);
                productList.Add(product);

                Console.WriteLine($"Added product: {product}");
            }

            return productList;
        }

    }
}
