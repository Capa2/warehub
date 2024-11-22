using FastEndpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warehub.model;
using warehub.model.interfaces;

namespace warehub.Endpoints.Products.GetProduct
{
    public class Mappers : Mapper<ProductRequest, ProductResponse, IProduct>
    {
        public override ProductResponse FromEntity(IProduct e)
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
