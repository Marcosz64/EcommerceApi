using ECommerce.Application.Orders.Commands;
using FluentValidation;

namespace ECommerce.Application.Orders.Validators;

public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("El usuario es obligatorio");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("La orden debe tener al menos un producto");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductId)
                .NotEmpty()
                .WithMessage("El producto es obligatorio");

            item.RuleFor(i => i.Quantity)
                .InclusiveBetween(1, 100)
                .WithMessage("La cantidad debe estar entre 1 y 100");
        });
    }
}
