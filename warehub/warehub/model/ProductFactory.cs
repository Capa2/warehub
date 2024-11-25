using System;
using warehub.model.interfaces;
using warehub.services;
using NLog;

namespace warehub.model
{
    public static class ProductFactory
    {
        /// <summary>
        /// Provides methods for creating product instances with specified properties.
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
