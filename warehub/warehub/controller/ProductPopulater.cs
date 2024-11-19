using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using warehub.model;
using warehub.repository;
using warehub.services;
using NLog;
using static warehub.controller.JsonCustomConverter;

namespace warehub.controller
{
    /// <summary>
    /// Handles the population of products from a JSON file into the data store.
    /// </summary>
    public static class ProductPopulater
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Populates the product data from a JSON file.
        /// </summary>
        public static void Populate()
        {
            string relativePath = "controller\\ExampleProducts.json"; // Path relative to the application root
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

            try
            {
                Logger.Trace($"ProductPopulator: Starting product population. File path: {filePath}");

                string jsonContent;

                try // Read the JSON file
                {
                    jsonContent = File.ReadAllText(filePath);
                    Logger.Trace("ProductPopulator: Read the JSON file successfully.");
                }
                catch (FileNotFoundException fnfEx)
                {
                    Logger.Error($"ProductPopulator: File not found: {filePath}. {fnfEx.Message}");
                    return;
                }
                catch (UnauthorizedAccessException uaEx)
                {
                    Logger.Error($"ProductPopulator: Access denied for file: {filePath}. {uaEx.Message}");
                    return;
                }
                catch (Exception ex)
                {
                    Logger.Error($"ProductPopulator: Error reading file: {ex.Message}");
                    return;
                }

                List<Product>? products;
                try // Deserialize the JSON content into Product objects
                {
                    JsonSerializerOptions options = new()
                    {
                        Converters = { new ProductConverter() },
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };
                    products = JsonSerializer.Deserialize<List<Product>>(jsonContent, options);
                    Logger.Trace("ProductPopulator: Deserialized JSON content into Product objects.");
                }
                catch (JsonException jsonEx)
                {
                    Logger.Error($"ProductPopulator: Deserialization failed. {jsonEx.Message}");
                    return;
                }
                catch (Exception ex)
                {
                    Logger.Error($"ProductPopulator: Unexpected deserialization error: {ex.Message}");
                    return;
                }

                if (products == null || products.Count == 0)
                {
                    Logger.Warn("ProductPopulator: No products found in the JSON file.");
                    return;
                }

                // Initialize the repository and service
                ProductRepository productRepository = new();
                ProductService productService = new ProductService(productRepository);

                try // Add products to the repository
                {
                    foreach (Product product in products)
                    {
                        productService.AddProduct(product);
                        Logger.Trace($"ProductPopulator: Added product: {product.Name} (ID: {product.Id})");
                    }
                    Logger.Info($"ProductPopulator: Added {products.Count} products to the repository");
                }
                catch (Exception ex)
                {
                    Logger.Error($"ProductPopulator: Error adding products: {ex.Message}");
                    return;
                }

                try // Retrieve and update products
                {
                    List<Product>? productsReturned = productService.GetAllProducts().Result;
                    if (productsReturned != null)
                    {
                        var productToUpdate = productsReturned.FirstOrDefault();
                        if (productToUpdate != null)
                        {
                            bool updateResult = productService.UpdateProduct(productToUpdate).Result;
                            Logger.Trace($"ProductPopulator: Product update result for {productToUpdate.Name}: {updateResult}");
                        }
                        else Logger.Warn("ProductPopulator: No products available to update.");
                    }
                    else Logger.Warn("ProductPopulator: Failed to retrieve products.");
                }
                catch (Exception ex)
                {
                    Logger.Error($"ProductPopulator: Error retrieving or updating products: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal($"ProductPopulator: Critical error in Populate: {ex.Message}");
            }
            finally
            {
                Logger.Trace("ProductPopulator: Product population process completed.");
            }
        }
    }
}