using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using warehub.model;
using warehub.repository;
using warehub.services;
using static warehub.controller.JsonCustomConverter;

namespace warehub.controller
{
    public static class ProductPopulater
    {
        public static void Populate()
        {

            string relativePath = "controller\\ExampleProducts.json"; // Path relative to the application root
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

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
                }
                var productsReturned = productService.GetAllProducts();
                if (productsReturned != null)
                {
                    var productToUpdate = productsReturned.FirstOrDefault();
                    var result = productService.UpdateProduct(productToUpdate);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
