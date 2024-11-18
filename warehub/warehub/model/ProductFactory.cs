using System;
using warehub.model.interfaces;
using warehub.services;
using NLog;

namespace warehub.model
{
    public static class ProductFactory
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static Product CreateProduct(string name, decimal price)
        {
            Logger.Trace(string.Join("Factory is creating product with name: ", name, ", and price:", price));
            return new Product(name, price);
        }

        public static Product CreateProduct(Guid id, string name, decimal price, int amount)
        {
            Logger.Trace(string.Join("Factory is creating product with id: ", id, ", name: ", name, ", and price:", price));
            return new Product(id, name, price, amount);
        }
    }
}
