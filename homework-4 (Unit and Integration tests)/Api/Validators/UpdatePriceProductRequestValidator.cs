using FluentValidation;

namespace Api.Validators;

public class UpdatePriceProductRequestValidator : AbstractValidator<UpdatePriceProductRequest>
{
    public UpdatePriceProductRequestValidator()
    {
        RuleFor(product => product.ProductId)
            .GreaterThanOrEqualTo(0)
            .WithMessage("No product with this id")
            .LessThanOrEqualTo(int.MaxValue)
            .WithMessage("No product with this id");
        
        RuleFor(product => product.UpdatedFields.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be positive")
            .LessThanOrEqualTo(1000000000)
            .WithMessage("Price must not exceed a billion");
    }
}