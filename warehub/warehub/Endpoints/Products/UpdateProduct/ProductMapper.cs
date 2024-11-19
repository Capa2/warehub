using FastEndpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warehub.model;

namespace warehub.Endpoints.Products.UpdateProduct
{
    public class Mappers : Mapper<ProductRequest, ProductResponse, Product>
    {
        public override ProductResponse FromEntity(Product e)
        {
            return new()
            {
                Id = e.Id,
                Name = e.Name,
                Price = e.Price,
                Amount = e.Amount
            };
        }
    }
}
