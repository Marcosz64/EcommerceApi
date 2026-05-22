using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Orders.Queries;

public record GetAllOrdersQuery(Guid UserId) : IRequest<IEnumerable<OrderResponseDto>>;
