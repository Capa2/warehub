using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using warehub.db;
using warehub.db.DTO;
using warehub.db.enums;
using warehub.model;
using warehub.repository.interfaces;
using warehub.repository.returnObjects;
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
            bool status = false;
            if (product.ValidateAttributesPresent())
            {
                ProductDTO productDTO = ObjectMapper.Map<Product, ProductDTO>(product);
                status = _cRUDService.Create<ProductDTO>(Table.Products, productDTO);
            }

            var returnObject = new GenericResponseDTO<Product>(product)
            {
                IsSuccess = status
            };
            return returnObject;
        }

        public GenericResponseDTO<Guid> Delete(Guid id)
        {
            bool status = _cRUDService.Delete(Table.Products, "id", id);
            var returnObject = new GenericResponseDTO<Guid>(id)
            {
                IsSuccess = status
            };
            return returnObject;
        }

        public GenericResponseDTO<List<Product>> GetAll()
        {
            var (status, products) = _cRUDService.Read<ProductDTO>(Table.Products, new Dictionary<string, object>());
            List<Product> listOfProducts = ConvertToProducts(products);
            var returnObject = new GenericResponseDTO<List<Product>>(listOfProducts)
            {
                IsSuccess = status
            };
            return returnObject;
        }

        public GenericResponseDTO<Product> GetById(Guid id)
        {
            var (status, products) = _cRUDService.Read<ProductDTO>(Table.Products, new Dictionary<string, object> { { "id", id } });
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
            bool status = _cRUDService.Update(Table.Products, updateParams, "id", product.Id);

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
            Type productType = typeof(Product);
            // Retrieve all the properties of the Product type
            PropertyInfo[] properties = productType.GetProperties();

            foreach (var productDict in products)
            {
                bool objectValid = true;
                foreach (var property in properties)
                {
                    if (!productDict.ContainsKey(property.Name) && productDict[property.Name].GetType() == property.PropertyType)
                    {
                        Console.WriteLine($"Skipping product due to invalid or missing '{property.Name}'.");
                        objectValid = false;
                        break;
                    }
                    
                }
                
                var product = ProductFactory.CreateProduct((Guid)productDict["id"], productDict["name"].ToString(), (int)productDict["price"]);
                Console.WriteLine($"Added product: {product}");
                productList.Add(product);

                
            }
            return productList;

        }
    }
}
