using FluentValidation;

namespace Api.Validators;

public class ListProductRequestValidator : AbstractValidator<ListProductRequest>
{
    public ListProductRequestValidator()
    {
        RuleFor(product => product.ProductType)
            .Must(field => field is ProductType.Common
                or ProductType.Householdchemicals
                or ProductType.Technic
                or ProductType.Products)
            .WithMessage("Order field must be from 0 to 3");
        
        RuleFor(product => product.CreationDate)
            .Must(field => field is null || field.Seconds >= 0)
            .WithMessage("Creation Date seconds must be positive")
            .Must(field => field is null || field.Nanos >= 0)
            .WithMessage("Creation Date nanos must be positive");
        
        RuleFor(product => product.WarehouseId)
            .GreaterThanOrEqualTo(0)
            .WithMessage("WarehouseId must be positive")
            .LessThanOrEqualTo(long.MaxValue)
            .WithMessage("Warehouse Id must be less");
        
        RuleFor(product => product.OrderField)
            .Must(field => field is ListProductRequest.Types.OrderField.Noorderfield
                or ListProductRequest.Types.OrderField.Creationdate
                or ListProductRequest.Types.OrderField.Type
                or ListProductRequest.Types.OrderField.Warehouseid)
            .WithMessage("Order field must be from 0 to 3");
        
        RuleFor(product => product.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be more than 0")
            .LessThanOrEqualTo(int.MaxValue)
            .WithMessage("Page must be less");
        
        RuleFor(product => product.Size)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Size must be more than 0")
            .LessThanOrEqualTo(10)
            .WithMessage("Size must be less than or equal to 10");
    }
}