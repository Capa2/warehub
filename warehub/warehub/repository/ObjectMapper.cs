using System;
using System.Collections.Generic;
using warehub.model;
using NLog;

namespace warehub.repository
{
    /// <summary>
    /// Provides methods for mapping dictionary data to Product objects.
    /// </summary>
    public static class ObjectMapper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Maps a list of dictionaries to a list of Product objects.
        /// </summary>
        /// <param name="products">The list of dictionaries containing product data.</param>
        /// <returns>A list of mapped Product objects.</returns>
        public static List<Product> MapDictToProducts(List<Dictionary<string, object>> products)
        {
            var productList = new List<Product>();
            foreach (var productDict in products)
            {
                // Validate and extract 'id'
                if (!productDict.TryGetValue("id", out var idObj) || idObj is not Guid id)
                {
                    Logger.Warn("Skipping product due to invalid or missing 'id'.");
                    continue;
                }

                // Validate and extract 'name'
                if (!productDict.TryGetValue("name", out var nameObj) || nameObj is not string name)
                {
                    Logger.Warn("Skipping product due to invalid or missing 'name'.");
                    continue;
                }

                // Validate and extract 'price'
                if (!productDict.TryGetValue("price", out var priceObj) || priceObj is not decimal price)
                {
                    Logger.Warn("Skipping product due to invalid or missing 'price'.");
                    continue;
                }

                // Validate and extract 'amount'
                if (!productDict.TryGetValue("amount", out var amountObj) || amountObj is not int amount)
                {
                    Logger.Warn("Skipping product due to invalid or missing 'Amount'.");
                    continue;
                }

                // Create and add the Product object
                try
                {
                    var product = ProductFactory.CreateProduct(id, name, price, amount);
                    productList.Add(product);
                    Logger.Trace($"Added product: {product.Name} (ID: {product.Id}, Price: {price}, Amount: {amount})");
                }
                catch (Exception ex)
                {
                    Logger.Error($"Error creating product with ID: {id}. {ex.Message}");
                }
            }

            Logger.Info($"Mapping completed. Total products added: {productList.Count}");
            return productList;
        }
    }
}
