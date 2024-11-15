using System;
using warehub.model.interfaces;
using warehub.services;

namespace warehub.model
{
    public static class ProductFactory
    {
        public static Product CreateProduct(string name, decimal price)
        {
            return new Product(name, price);
        }

        public static Product CreateProduct(Guid id, string name, decimal price)
        {
            return new Product(id, name, price);
        }
    }
}
