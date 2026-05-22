using ECommerce.Application.Auth.Commands;
using FluentValidation;

namespace ECommerce.Application.Auth.Validators;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio")
            .MaximumLength(100)
            .WithMessage("El nombre no puede superar los 100 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("El email es obligatorio")
            .EmailAddress()
            .WithMessage("El email no tiene un formato válido")
            .MaximumLength(250)
            .WithMessage("El email no puede superar los 250 caracteres");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("La contraseña es obligatoria")
            .MinimumLength(6)
            .WithMessage("La contraseña debe tener al menos 6 caracteres")
            .MaximumLength(100)
            .WithMessage("La contraseña no puede superar los 100 caracteres")
            .Matches("[A-Z]")
            .WithMessage("La contraseña debe contener al menos una letra mayúscula")
            .Matches("[a-z]")
            .WithMessage("La contraseña debe contener al menos una letra minúscula")
            .Matches("[0-9]")
            .WithMessage("La contraseña debe contener al menos un número");
    }
}