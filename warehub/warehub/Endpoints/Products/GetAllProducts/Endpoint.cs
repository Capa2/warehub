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

namespace warehub.Endpoints.Products.GetAllProducts
{
    public class Endpoint : Endpoint<EmptyRequest, ProductsResponse, Mappers>
    {
        private readonly IProductService _productService;

        public Endpoint(IProductService productService)
        {
            _productService = productService;
        }
        public override void Configure()
        {
            Get("/api/Products/");
            AllowAnonymous();
        }

        public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
        {
            var products = await _productService.GetAllProducts();

            if (products == null)
            {
                await SendNotFoundAsync(ct); // Respond with 404 if no products found
                return;
            }

            // Map the entity to the response DTO and send the response
            var response = Mappers.FromEntity(products);
            await SendAsync(response, cancellation: ct);
        }
    }
}
