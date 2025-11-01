using FarmManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.Animals.Queries
{
    public class GetAllAnimalsQuery : IRequest<List<Animal>>
    {
        public bool IncludeRelatedData { get; set; } = false;
    }

    public class GetAllAnimalsQueryHandler : IRequestHandler<GetAllAnimalsQuery, List<Animal>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllAnimalsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Animal>> Handle(GetAllAnimalsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Animals.AsQueryable();

            if (request.IncludeRelatedData)
            {
                query = query
                    .Include(a => a.Sire)
                    .Include(a => a.Dam)
                    .Include(a => a.WeightRecords)
                    .Include(a => a.BirthRecordsAsMother)
                    .Include(a => a.BreedingRecords)
                    .Include(a => a.HealthRecords);
            }

            return await query
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
