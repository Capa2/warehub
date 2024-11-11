using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warehub.services.interfaces;

namespace warehub.model
{
    internal abstract class Product
    {
        private Guid Id { get; }
        private string Name { get; }
        private int Price { get; }

        Product(IIdService idService, string name, int price)
        {
            Id = idService.GenerateId();
            Name = name;
            Price = price;
        }
        Product(Guid id, string name, int price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
    }
}
