using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Orders.Commands;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<Guid> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var order = new Order(request.UserId);

        foreach (var item in request.Items)
        {
            var product = await _productRepository.GetByIdAsync(
                item.ProductId,
                cancellationToken);

            if (product is null)
                throw new InvalidOperationException($"Producto con ID {item.ProductId} no encontrado.");

            order.AddItem(product, item.Quantity);

            await _productRepository.UpdateAsync(product, cancellationToken);
        }

        await _orderRepository.AddAsync(order, cancellationToken);

        return order.Id;
    }
}
