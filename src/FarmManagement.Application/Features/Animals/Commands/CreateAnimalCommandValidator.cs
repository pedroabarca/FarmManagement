using FluentValidation;

namespace FarmManagement.Application.Features.Animals.Commands
{
    public class CreateAnimalCommandValidator : AbstractValidator<CreateAnimalCommand>
    {
        public CreateAnimalCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del animal es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede tener más de 100 caracteres.");

            RuleFor(x => x.Species)
                .NotEmpty().WithMessage("La especie es obligatoria.")
                .MaximumLength(50).WithMessage("La especie no puede tener más de 50 caracteres.");
        }
    }
}
