using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Products.Queries;

public record GetAllProductsQuery() : IRequest<IEnumerable<ProductResponseDto>>;