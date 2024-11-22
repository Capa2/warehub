using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace warehub.model.interfaces
{
    public interface IProduct
    {
        int Amount { get; }
        Guid Id { get; }
        string Name { get; }
        decimal Price { get; }
        string ToString();
    }
}
