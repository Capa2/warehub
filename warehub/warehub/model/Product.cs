using System;
using warehub.model.interfaces;
using warehub.services;

namespace warehub.model
{
    public class Product : IProduct
    {
        public Guid Id { get; }
        public string Name { get; }
        public decimal Price { get; }

        public Product(string name, decimal price)
        {
            Id = GuidService.GenerateId();
            Name = name;
            Price = price;
        }

        public Product(Guid id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        public override string ToString()
        {
            return $"Product: {Name}, Price: {Price}, ID: {Id}";
        }
    }
}
