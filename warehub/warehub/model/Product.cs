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
        public int Amount { get; }

        public Product(string name, int price, int amount)
        {
            Id = GuidService.GenerateId();
            Name = name;
            Price = price;
            Amount = amount;
        }

        public Product(Guid id, string name, int price, int amount)
        {
            Id = id;
            Name = name;
            Price = price;
            Amount = amount;
        }

        public override string ToString()
        {
            return $"Product: {Name}, Price: {Price}, ID: {Id}, Amount {Amount}";
        }
    }
}
