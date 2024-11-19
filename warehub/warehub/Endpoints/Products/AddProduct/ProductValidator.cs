using FastEndpoints;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace warehub.Endpoints.Products.AddProduct
{
    public class ProductValidator: Validator<ProductRequest>
    {
        public ProductValidator() 
        {
            RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("The 'Name' field is required."); // Validate Name is not empty

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("The 'Price' must be greater than zero."); // Validate Price is positive

            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("The 'Amount' must be zero or a positive integer."); // Validate Amount is non-negative

        }
    }
}
