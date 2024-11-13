using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warehub.model;
using warehub.services.interfaces;

namespace warehub.db
{
    public class ProductRepository(DbConnection dbConnection)
    {
        private readonly CRUDService _cRUDService = new(dbConnection.GetConnection());

        public GenericResponseDTO<Product> Add(Product product)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@name", product.Name },
                { "@price", product.Price },
                { "@id", product.Id }
            };
            bool status = _cRUDService.Create("INSERT INTO Products (Name, Price, Id) VALUES (@name, @price, @id)", parameters);
            var returnObject = new GenericResponseDTO<Product>(product)
            {
                IsSuccess = status
            };
            return returnObject;
        }

        public GenericResponseDTO<Guid> Delete(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@id", id }
            };
            bool status = _cRUDService.Delete("DELETE FROM Products WHERE ID = @id", parameters);
            var returnObject = new GenericResponseDTO<Guid>(id)
            {
                IsSuccess = status
            };
            return returnObject;
        }

        public GenericResponseDTO<List<Product>> GetAll()
        {
            (bool status, List<Dictionary<string, object>> products) = _cRUDService.Read("SELECT * FROM Products");
            List<Product> listOfProducts = ConvertToProducts(products);
            var returnObject = new GenericResponseDTO<List<Product>>(listOfProducts)
            {
                IsSuccess = status
            };
            return returnObject;
        }

        public GenericResponseDTO<Product> GetById(Guid id)
        {
            (bool status, List<Dictionary<string, object>> products) = _cRUDService.Read($"SELECT * FROM Products WHERE ID = {id}");
            List<Product> listOfProducts = ConvertToProducts(products);
            Product product = listOfProducts[0];
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
                { "@name", product.Name },
                { "@price", product.Price },
                { "@id", product.Id }
            };
            bool status = _cRUDService.Update("UPDATE Products SET Name = @name, Price = @price WHERE ID = @id", updateParams);

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
                // Get each property from the dictionary and cast to the correct type
                Guid id = productDict.ContainsKey("Id") && productDict["Id"] is Guid
                    ? (Guid)productDict["Id"]
                    : Guid.NewGuid();  // Generate new ID if none is found

                string name = productDict.ContainsKey("Name") && productDict["Name"] is string
                    ? (string)productDict["Name"]
                    : string.Empty;

                int price = productDict.ContainsKey("Price") && productDict["Price"] is int
                    ? (int)productDict["Price"]
                    : 0;

                // Create new Product instance with parsed values
                var product = new Product(id, name, price);
                productList.Add(product);
            }

            return productList;
        }
    }
}


