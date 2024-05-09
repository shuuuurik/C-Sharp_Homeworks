using Api.Application;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Api.Services;

public class ProductServiceGrpc : ProductService.ProductServiceBase
{
    private IApplicationProductService _productService;

    public ProductServiceGrpc(IApplicationProductService productService)
    {
        _productService = productService;
    }

    public override Task<AddProductResponse> Add(AddProductRequest request, ServerCallContext context)
    {
        var dto = new ProductDto (
            request.NewProduct.Name,
            request.NewProduct.Price,
            request.NewProduct.Weight,
            request.NewProduct.Type,
            request.NewProduct.WarehouseId
        );
        var productId = _productService.Add(dto);
        return Task.FromResult(
            new AddProductResponse
            {
                Id = productId
            });
    }

    public override Task<GetProductResponse> Get(GetProductRequest request, ServerCallContext context)
    {
        var product = _productService.Get(request.ProductId);
        return Task.FromResult(
            new GetProductResponse
            {
                Product = new ProductProto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Weight = product.Weight,
                    Type = (ProductType)product.Type,
                    CreationDate = DateTime
                        .SpecifyKind(product.CreationDate.ToUniversalTime(), DateTimeKind.Utc)
                        .ToTimestamp(),
                    WarehouseId = product.WarehouseId
                }
            });
    }

    public override Task<ListProductResponse> List(ListProductRequest request, ServerCallContext context)
    {
        var tuple = _productService.List(request.ProductType, request.CreationDate?.ToDateTime(), request.WarehouseId, request.OrderField, request.Page, request.Size);
        var productsOnPage = tuple.Item1;
        var filteredProductsCount = tuple.Item2;
        return Task.FromResult(
            new ListProductResponse
            {
                Products =
                {
                    productsOnPage.Select(p => new ProductProto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        Weight = p.Weight,
                        Type = (ProductType)p.Type,
                        CreationDate = DateTime
                            .SpecifyKind(p.CreationDate.ToUniversalTime(), DateTimeKind.Utc)
                            .ToTimestamp(),
                        WarehouseId = p.WarehouseId
                    })
                },
                FilteredProductsCount = filteredProductsCount,
                Page = request.Page
            });
    }

    public override Task<UpdatePriceProductResponse> UpdatePrice(UpdatePriceProductRequest request, ServerCallContext context)
    {
        var product = _productService.UpdatePrice(request.ProductId, request.UpdatedFields.Price);
        return Task.FromResult(
            new UpdatePriceProductResponse
            {
                Product = new ProductProto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Weight = product.Weight,
                    Type = (ProductType)product.Type,
                    CreationDate = DateTime
                        .SpecifyKind(product.CreationDate.ToUniversalTime(), DateTimeKind.Utc)
                        .ToTimestamp(),
                    WarehouseId = product.WarehouseId
                }
            });
    }
}