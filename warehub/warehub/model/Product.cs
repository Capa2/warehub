using System;
using warehub.model.interfaces;
using warehub.services;

namespace warehub.model
{
    public class Product : IProduct
    {
        public Guid Id { get; }
        public string Name { get; }
        public int Price { get; }

        public Product(string name, int price)
        {
            Id = GuidService.GenerateId();
            Name = name;
            Price = price;
        }

        public Product(Guid id, string name, int price)
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
