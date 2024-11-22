using FastEndpoints;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warehub.model;
using warehub.repository;
using warehub.services;
using warehub.services.interfaces;

namespace warehub.Endpoints.Products.GetProduct
{
    public class Endpoint : Endpoint<ProductRequest, ProductResponse, Mappers>
    {
        private readonly IProductService _productService;

        public Endpoint(IProductService productService)
        {
            _productService = productService;
        }
        public override void Configure()
        {
            Get("/api/Product/{ProductId}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(ProductRequest req, CancellationToken ct)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(req.ProductId))
            {
                ThrowError("Product ID is missing from the request");
            }

            // Attempt to parse the ProductId
            if (!Guid.TryParse(req.ProductId, out Guid productId))
            {
                ThrowError("Invalid Product ID format");
            }

            // Simulate fetching the product from a data source
            var product = await _productService.GetProductByIdAsync(productId);

            if (product == null)
            {
                await SendNotFoundAsync(ct); // Respond with 404 if product not found
                return;
            }

            // Map the entity to the response DTO and send the response
            var response = Map.FromEntity(product);
            await SendAsync(response, cancellation: ct);
        }
    }
}
