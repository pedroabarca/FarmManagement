using FarmManagement.Domain.Entities;
using FarmManagement.Domain.Enums;
using MediatR;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.Animals.Commands
{
    public class CreateAnimalCommand : IRequest<Animal>
    {
        // Required fields
        public string TagId { get; set; } = string.Empty;        // Unique physical tag
        public string Breed { get; set; } = string.Empty;
        public Sex Sex { get; set; }
        public DateTime BirthDate { get; set; }

        // Optional identification
        public string? ElectronicId { get; set; }
        public string? Name { get; set; }

        // Optional birth/weaning data
        public decimal? BirthWeightKg { get; set; }
        public decimal? WeaningWeightKg { get; set; }
        public DateTime? WeaningDate { get; set; }

        // Optional genealogy
        public int? SireId { get; set; }
        public int? DamId { get; set; }

        // Optional management data
        public string? CurrentLocation { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public decimal? PurchasePrice { get; set; }
        public string? Notes { get; set; }
    }

    public class CreateAnimalCommandHandler : IRequestHandler<CreateAnimalCommand, Animal>
    {
        private readonly IApplicationDbContext _context;

        public CreateAnimalCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Animal> Handle(CreateAnimalCommand request, CancellationToken cancellationToken)
        {
            var animal = new Animal
            {
                TagId = request.TagId,
                ElectronicId = request.ElectronicId,
                Name = request.Name,
                Breed = request.Breed,
                Sex = request.Sex,
                BirthDate = request.BirthDate,
                BirthWeightKg = request.BirthWeightKg,
                WeaningWeightKg = request.WeaningWeightKg,
                WeaningDate = request.WeaningDate,
                SireId = request.SireId,
                DamId = request.DamId,
                Status = AnimalStatus.Active,  // Default status
                CurrentLocation = request.CurrentLocation,
                PurchaseDate = request.PurchaseDate,
                PurchasePrice = request.PurchasePrice,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Animals.Add(animal);
            await _context.SaveChangesAsync(cancellationToken);

            return animal;
        }
    }
}
