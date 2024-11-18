using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warehub.model;

namespace warehub.repository
{
    public static class ObjectMapper
    {
        public static List<Product> MapDictToProducts(List<Dictionary<string, object>> products)
        {
            var productList = new List<Product>();
            foreach (var productDict in products)
            {
                if (!productDict.TryGetValue("id", out var idObj) || idObj is not Guid id)
                {
                    Console.WriteLine("Skipping product due to invalid or missing 'id'.");
                    continue;
                }

                if (!productDict.TryGetValue("name", out var nameObj) || nameObj is not string name)
                {
                    Console.WriteLine("Skipping product due to invalid or missing 'name'.");
                    continue;
                }

                if (!productDict.TryGetValue("price", out var priceObj) || priceObj is not decimal price)
                {
                    Console.WriteLine("Skipping product due to invalid or missing 'price'.");
                    continue;
                }

                if (!productDict.TryGetValue("amount", out var amountObj) || amountObj is not int amount)
                {
                    Console.WriteLine("Skipping product due to invalid or missing 'Amount'.");
                    continue;
                }

                var product = ProductFactory.CreateProduct(id, name, price, amount);
                productList.Add(product);

                Console.WriteLine($"Added product: {product}");
            }

            return productList;
        }
    }
}
