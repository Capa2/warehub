using warehub.model.interfaces;
using NLog;
using warehub.utils;
namespace warehub.model
{
    public class Product : IProduct
    {
        /// <summary>
        /// Represents a product in the system, encapsulating its unique identifier, name, price, and available amount.
        /// Provides constructors for creating new products and initializing existing ones.
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public Guid Id { get; }
        public string Name { get; }
        public decimal Price { get; }
        public int Amount { get; }

        public Product(string name, decimal price, int amount)
        {
            Id = GuidUtil.GenerateId();
            Name = name;
            Price = price;
            Amount = amount;
        }

        public Product(Guid id, string name, decimal price, int amount)
        {

            Id = id;
            Name = name;
            Price = price;
            Amount = amount;
            Logger.Trace("Product: Initialized " + this.ToString());
        }

        public override string ToString()
        {
            return $"Product: {Name}, Price: {Price}, ID: {Id}, Amount {Amount}";
        }
    }
}
