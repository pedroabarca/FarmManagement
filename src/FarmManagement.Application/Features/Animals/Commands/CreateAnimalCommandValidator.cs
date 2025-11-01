using FluentValidation;

namespace FarmManagement.Application.Features.Animals.Commands
{
    public class CreateAnimalCommandValidator : AbstractValidator<CreateAnimalCommand>
    {
        public CreateAnimalCommandValidator()
        {
            RuleFor(x => x.TagId)
                .NotEmpty().WithMessage("TagId is required.")
                .MaximumLength(50).WithMessage("TagId cannot exceed 50 characters.");

            RuleFor(x => x.Breed)
                .NotEmpty().WithMessage("Breed is required.")
                .MaximumLength(100).WithMessage("Breed cannot exceed 100 characters.");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("Birth date is required.")
                .LessThan(DateTime.UtcNow).WithMessage("Birth date cannot be in the future.");

            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.ElectronicId)
                .MaximumLength(50).WithMessage("Electronic ID cannot exceed 50 characters.")
                .When(x => !string.IsNullOrEmpty(x.ElectronicId));

            RuleFor(x => x.BirthWeightKg)
                .GreaterThan(0).WithMessage("Birth weight must be greater than 0.")
                .When(x => x.BirthWeightKg.HasValue);

            RuleFor(x => x.WeaningWeightKg)
                .GreaterThan(0).WithMessage("Weaning weight must be greater than 0.")
                .When(x => x.WeaningWeightKg.HasValue);

            RuleFor(x => x.PurchasePrice)
                .GreaterThanOrEqualTo(0).WithMessage("Purchase price cannot be negative.")
                .When(x => x.PurchasePrice.HasValue);
        }
    }
}
