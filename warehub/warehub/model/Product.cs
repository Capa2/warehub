using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warehub.services.interfaces;

namespace warehub.model
{
    public class Product
    {
        public Guid Id { get; }
        public string Name { get; }
        public int Price { get; }

        Product(IIdService idService, string name, int price)
        {
            Id = idService.GenerateId();
            Name = name;
            Price = price;
        }
        public Product(Guid id, string name, int price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
    }
}
