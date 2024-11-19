using FastEndpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warehub.model;

namespace warehub.Endpoints.Products.GetAllProducts
{
    public class Mappers : Mapper<EmptyRequest, ProductsResponse, Product>
    {

        // Mapping a collection of product entities to a list of response DTOs
        public static ProductsResponse FromEntity(IEnumerable<Product> entities)
        {
            ProductsResponse response = new ProductsResponse();
            response.Products.AddRange(entities.Select(entity => new ProductResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price,
                Amount = entity.Amount
            }).ToList());
            return response;
        }
    }
}
