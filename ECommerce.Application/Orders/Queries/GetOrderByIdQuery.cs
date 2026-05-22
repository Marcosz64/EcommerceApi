using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Orders.Queries;

public record GetOrderByIdQuery(Guid OrderId) : IRequest<OrderResponseDto?>;
