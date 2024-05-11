using FluentValidation;

namespace Api.Validators;

public class AddProductRequestValidator : AbstractValidator<AddProductRequest>
{
    public AddProductRequestValidator()
    {
        RuleFor(product => product.NewProduct.Name)
            .MinimumLength(1)
            .WithMessage("Product name is empty")
            .MaximumLength(30)
            .WithMessage("Product name max length is 30 characters");
        
        RuleFor(product => product.NewProduct.Price)
            .GreaterThan(0)
            .WithMessage("Price must be more than 0")
            .LessThanOrEqualTo(1000000000)
            .WithMessage("Price must not exceed a billion");
        
        RuleFor(product => product.NewProduct.Weight)
            .GreaterThan(0)
            .WithMessage("Weight must be more than 0")
            .LessThanOrEqualTo(1000)
            .WithMessage("Weight must not exceed 1000");

        RuleFor(product => product.NewProduct.Type)
            .Must(type => type == ProductType.Common || type == ProductType.Householdchemicals 
                                                     || type == ProductType.Technic 
                                                     || type == ProductType.Products)
            .WithMessage("Type must be from 0 to 3");

        RuleFor(product => product.NewProduct.WarehouseId)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Warehouse Id must be positive")
            .LessThanOrEqualTo(long.MaxValue)
            .WithMessage("Warehouse Id must be less");
    }
}