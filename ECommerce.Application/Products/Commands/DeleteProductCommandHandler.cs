using ECommerce.Application.Interfaces;
using MediatR;

namespace ECommerce.Application.Products.Commands;

public class DeleteProductCommandHandler
    : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<bool> Handle(
        DeleteProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
            return false;

        await _productRepository.DeleteAsync(request.Id, cancellationToken);

        return true;
    }
}
