using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using MediatR;

namespace ECommerce.Application.Products.Commands;

public class UpdateProductCommandHandler
    : IRequestHandler<UpdateProductCommand, ProductResponseDto?>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductResponseDto?> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
            return null;

        product.Update(
            request.Name,
            request.Description,
            request.Price,
            request.Stock
        );

        await _productRepository.UpdateAsync(product, cancellationToken);

        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock
        };
    }
}
