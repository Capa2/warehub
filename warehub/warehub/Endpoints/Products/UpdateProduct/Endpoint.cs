using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warehub.model;
using warehub.repository;
using warehub.services;
using warehub.services.interfaces;

namespace warehub.Endpoints.Products.UpdateProduct
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
            Put("/api/Product/");
            AllowAnonymous();
            Validator<ProductValidator>();
        }

        public override async Task HandleAsync(ProductRequest req, CancellationToken ct)
        {
            // Validate input
            Product product = ProductFactory.CreateProduct(req.Id, req.Name, req.Price, req.Amount);

            // Simulate fetching the product from a data source
            var addResponse = await _productService.UpdateProduct(product);

            if (!addResponse)
            {
                ThrowError("Error");
            }

            // Map the entity to the response DTO and send the response
            var response = Map.FromEntity(product);
            await SendAsync(response, cancellation: ct);
        }
    }
}
