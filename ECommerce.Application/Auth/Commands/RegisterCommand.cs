using MediatR;

namespace ECommerce.Application.Auth.Commands;

public record RegisterCommand(
    string Name,
    string Email,
    string Password
) : IRequest<string?>;
