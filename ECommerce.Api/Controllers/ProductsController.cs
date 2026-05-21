using ECommerce.Application.DTOs;
using ECommerce.Application.Products.Commands;
using ECommerce.Application.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var products = await _mediator.Send(new GetAllProductsQuery(), ct);
        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id), ct);

        if (product is null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto, CancellationToken ct)
    {
        var command = new CreateProductCommand(
            dto.Name,
            dto.Description,
            dto.Price,
            dto.Stock
        );

        var created = await _mediator.Send(command, ct);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDto dto, CancellationToken ct)
    {
        var command = new UpdateProductCommand(
            id,
            dto.Name,
            dto.Description,
            dto.Price,
            dto.Stock
        );

        var updated = await _mediator.Send(command, ct);

        if (updated is null)
            return NotFound();

        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var command = new DeleteProductCommand(id);

        var deleted = await _mediator.Send(command, ct);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}