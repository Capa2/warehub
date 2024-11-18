using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using warehub.db;
using warehub.model;
using warehub.services;
using static warehub.controller.JsonCustomConverter;

namespace warehub.controller
{
    public static class ProductPopulater
    {
        public static void Populate()
        {

            string filePath = "C:\\Users\\spac-36\\Source\\Repos\\warehub\\warehub\\warehub\\controller\\ExampleProducts.json";

            try
            {
                // Read the JSON file
                string jsonContent = File.ReadAllText(filePath);

                // Deserialize the JSON into a list of Product objects
                var options = new JsonSerializerOptions
                {
                    Converters = { new ProductConverter() },
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                List<Product> products = JsonSerializer.Deserialize<List<Product>>(jsonContent, options);
                ProductRepository productRepository = new ProductRepository();
                ProductService productService = new ProductService(productRepository);
                // Output the products
                foreach (var product in products)
                {
                    productService.AddProduct(product);
                    Console.WriteLine($"Name: {product.Name}, Price: {product.Price}, Amount: {product.Amount}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
