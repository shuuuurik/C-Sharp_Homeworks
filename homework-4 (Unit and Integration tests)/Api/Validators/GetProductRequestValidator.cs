using FluentValidation;
using Infrastructure;

namespace Api.Validators;

public class GetProductRequestValidator : AbstractValidator<GetProductRequest>
{
    public GetProductRequestValidator()
    {
        RuleFor(product => product.ProductId)
            .GreaterThanOrEqualTo(0)
            .WithMessage("No product with this id")
            .LessThanOrEqualTo(int.MaxValue)
            .WithMessage("No product with this id");
    }
}