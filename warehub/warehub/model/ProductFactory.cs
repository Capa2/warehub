using System;
using warehub.model.interfaces;
using warehub.services;
using NLog;

namespace warehub.model
{
    public static class ProductFactory
    {
        /// <summary>
        /// Provides factory methods for creating new product instances with specified attributes.
        /// Supports creation of products with or without predefined unique identifiers.
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static Product CreateProduct(string name, decimal price, int amount)
        {
            Logger.Trace($"ProductFactory: Created product - name: {name}, price: {price}");
            return new Product(name, price, amount);
        }

        public static Product CreateProduct(Guid id, string name, decimal price, int amount)
        {
            Logger.Trace($"ProductFactory: Created product - id: {id}, name: {name}, price: {price}");
            return new Product(id, name, price, amount);
        }
    }
}
