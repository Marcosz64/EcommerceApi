using System.Security.Claims;
using ECommerce.Application.Orders.Commands;
using ECommerce.Application.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdValue is null)
            return Unauthorized();

        var userId = Guid.Parse(userIdValue);

        var orders = await _mediator.Send(new GetAllOrdersQuery(userId), ct);
        return Ok(orders);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdValue is null)
            return Unauthorized();

        var order = await _mediator.Send(new GetOrderByIdQuery(id), ct);

        if (order is null)
            return NotFound();

        // Validar que la orden pertenece al usuario autenticado
        var userId = Guid.Parse(userIdValue);
        if (order.UserId != userId)
            return Forbid();

        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateOrderRequest request,
        CancellationToken ct)
    {
        var userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdValue is null)
            return Unauthorized();

        var userId = Guid.Parse(userIdValue);

        var command = new CreateOrderCommand(
            userId,
            request.Items.Select(i => new OrderItemDto(
                i.ProductId,
                i.Quantity
            )).ToList()
        );

        var orderId = await _mediator.Send(command, ct);

        return CreatedAtAction(nameof(Create), new { id = orderId }, new
        {
            id = orderId
        });
    }
}

public class CreateOrderRequest
{
    public List<CreateOrderItemRequest> Items { get; set; } = new();
}

public class CreateOrderItemRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
