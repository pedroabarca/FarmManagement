using FarmManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.BreedingRecords.Queries
{
    public class GetAllBreedingRecordsQuery : IRequest<List<BreedingRecord>>
    {
        public bool IncludeRelatedData { get; set; } = false;
    }

    public class GetAllBreedingRecordsQueryHandler : IRequestHandler<GetAllBreedingRecordsQuery, List<BreedingRecord>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllBreedingRecordsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BreedingRecord>> Handle(GetAllBreedingRecordsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.BreedingRecords.AsQueryable();

            if (request.IncludeRelatedData)
            {
                query = query
                    .Include(b => b.Animal)
                    .Include(b => b.Sire);
            }

            return await query
                .OrderByDescending(b => b.EventDate)
                .ToListAsync(cancellationToken);
        }
    }
}
