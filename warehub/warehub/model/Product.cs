using System;
using warehub.model.interfaces;
using warehub.services;
using NLog;
namespace warehub.model
{
    public class Product : IProduct
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public Guid Id { get; }
        public string Name { get; }
        public int Price { get; }

        public Product(string name, int price)
        {
            Id = GuidService.GenerateId();
            Name = name;
            Price = price;
            Logger.Trace("Initialized " + this.ToString);
        }

        public Product(Guid id, string name, int price)
        {
            
            Id = id;
            Name = name;
            Price = price;
            Logger.Trace("Initialized " + this.ToString);
        }

        public override string ToString()
        {
            return $"Product: {Name}, Price: {Price}, ID: {Id}";
        }
    }
}
