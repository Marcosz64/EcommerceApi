using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Products.Commands;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int Stock
) : IRequest<ProductResponseDto?>;
