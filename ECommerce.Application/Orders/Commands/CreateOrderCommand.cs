using MediatR;

namespace ECommerce.Application.Orders.Commands;

public record CreateOrderCommand(
    Guid UserId,
    List<OrderItemDto> Items
) : IRequest<Guid>;

public record OrderItemDto(
    Guid ProductId,
    int Quantity
);
