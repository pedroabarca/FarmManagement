using FarmManagement.Domain.Entities;
using FarmManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.Animals.Commands
{
    public class UpdateAnimalCommand : IRequest<Animal>
    {
        public int Id { get; set; }

        // Identity
        public string TagId { get; set; } = string.Empty;
        public string? ElectronicId { get; set; }
        public string? Name { get; set; }

        // Basic info
        public string Breed { get; set; } = string.Empty;
        public Sex Sex { get; set; }
        public DateTime BirthDate { get; set; }
        public decimal? BirthWeightKg { get; set; }
        public decimal? WeaningWeightKg { get; set; }
        public DateTime? WeaningDate { get; set; }

        // Genealogy
        public int? SireId { get; set; }
        public int? DamId { get; set; }

        // Reproductive tracking
        public DateTime? LastCalvingDate { get; set; }
        public DateTime? LastHeatDate { get; set; }
        public DateTime? LastBreedingDate { get; set; }
        public DateTime? LastPalpationDate { get; set; }
        public DateTime? NextExpectedCalvingDate { get; set; }

        // Management
        public AnimalStatus Status { get; set; }
        public string? CurrentLocation { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public decimal? PurchasePrice { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateAnimalCommandHandler : IRequestHandler<UpdateAnimalCommand, Animal>
    {
        private readonly IApplicationDbContext _context;

        public UpdateAnimalCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Animal> Handle(UpdateAnimalCommand request, CancellationToken cancellationToken)
        {
            var animal = await _context.Animals
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (animal == null)
            {
                throw new KeyNotFoundException($"Animal with ID {request.Id} not found.");
            }

            // Update all fields
            animal.TagId = request.TagId;
            animal.ElectronicId = request.ElectronicId;
            animal.Name = request.Name;
            animal.Breed = request.Breed;
            animal.Sex = request.Sex;
            animal.BirthDate = request.BirthDate;
            animal.BirthWeightKg = request.BirthWeightKg;
            animal.WeaningWeightKg = request.WeaningWeightKg;
            animal.WeaningDate = request.WeaningDate;
            animal.SireId = request.SireId;
            animal.DamId = request.DamId;
            animal.LastCalvingDate = request.LastCalvingDate;
            animal.LastHeatDate = request.LastHeatDate;
            animal.LastBreedingDate = request.LastBreedingDate;
            animal.LastPalpationDate = request.LastPalpationDate;
            animal.NextExpectedCalvingDate = request.NextExpectedCalvingDate;
            animal.Status = request.Status;
            animal.CurrentLocation = request.CurrentLocation;
            animal.PurchaseDate = request.PurchaseDate;
            animal.PurchasePrice = request.PurchasePrice;
            animal.Notes = request.Notes;
            animal.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return animal;
        }
    }
}
