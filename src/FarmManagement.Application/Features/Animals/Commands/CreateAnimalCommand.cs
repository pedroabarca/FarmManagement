using FarmManagement.Domain.Entities;
using MediatR;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.Animals.Commands
{
    public class CreateAnimalCommand : IRequest<Animal>
    {
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
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
                Name = request.Name,
                Species = request.Species
            };

            _context.Animals.Add(animal);
            await _context.SaveChangesAsync(cancellationToken);

            return animal;
        }
    }
}
