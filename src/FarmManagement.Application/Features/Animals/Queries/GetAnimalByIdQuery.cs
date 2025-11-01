using FarmManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.Animals.Queries
{
    public class GetAnimalByIdQuery : IRequest<Animal?>
    {
        public int Id { get; set; }
    }

    public class GetAnimalByIdQueryHandler : IRequestHandler<GetAnimalByIdQuery, Animal?>
    {
        private readonly IApplicationDbContext _context;

        public GetAnimalByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Animal?> Handle(GetAnimalByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Animals
                .Include(a => a.Sire)
                .Include(a => a.Dam)
                .Include(a => a.WeightRecords)
                .Include(a => a.BirthRecordsAsMother)
                .Include(a => a.BreedingRecords)
                .Include(a => a.HealthRecords)
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        }
    }
}
