using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace warehub.Endpoints.Products.GetAllProducts
{
    public class ProductsResponse
    {
        public List<ProductResponse> Products { get; set; } = [];
    }
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
    }
    
}
