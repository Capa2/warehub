using System;
using warehub.model.interfaces;
using warehub.services;

namespace warehub.model
{
    public static class ProductFactory
    {
        public static Product CreateProduct(string name, decimal price, int amount)
        {
            return new Product(name, price, amount);
        }

        public static Product CreateProduct(Guid id, string name, decimal price, int amount)
        {
            return new Product(id, name, price, amount);
        }
    }
}
