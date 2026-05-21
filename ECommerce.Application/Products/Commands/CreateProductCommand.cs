using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Products.Commands;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int Stock
) : IRequest<ProductResponseDto>;