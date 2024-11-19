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

namespace warehub.Endpoints.Products.DeleteProduct
{
    public class Endpoint : Endpoint<ProductRequest, EmptyResponse>
    {
        private readonly ProductService _productService;

        public Endpoint(ProductService productService)
        {
            _productService = productService;
        }
        public override void Configure()
        {
            Delete("/api/Product/{ProductId}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(ProductRequest req, CancellationToken ct)
        {
            // Simulate fetching the product from a data source
            var deleteResponse = await _productService.DeleteProduct(req.ProductId);

            if (!deleteResponse)
            {
                await SendNotFoundAsync(ct); // Respond with 404 if product not found
                return;
            }

            // Map the entity to the response DTO and send the response
            await SendOkAsync(cancellation: ct);
        }
    }
}
