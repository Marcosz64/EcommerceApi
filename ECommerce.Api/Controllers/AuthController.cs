using ECommerce.Application.Auth.Commands;
using ECommerce.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequestDto request,
        CancellationToken ct)
    {
        var token = await _mediator.Send(
            new LoginCommand(request.Email, request.Password),
            ct);

        if (token is null)
            return Unauthorized(new { message = "Credenciales incorrectas" });

        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequestDto request,
        CancellationToken ct)
    {
        var token = await _mediator.Send(
            new RegisterCommand(request.Name, request.Email, request.Password),
            ct);

        if (token is null)
            return Conflict(new { message = "El email ya está registrado" });

        return Ok(new { token });
    }
}