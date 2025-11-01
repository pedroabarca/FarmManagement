using FarmManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.HealthRecords.Queries
{
    public class GetAllHealthRecordsQuery : IRequest<List<HealthRecord>>
    {
        public bool IncludeAnimal { get; set; } = false;
    }

    public class GetAllHealthRecordsQueryHandler : IRequestHandler<GetAllHealthRecordsQuery, List<HealthRecord>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllHealthRecordsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<HealthRecord>> Handle(GetAllHealthRecordsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.HealthRecords.AsQueryable();

            if (request.IncludeAnimal)
            {
                query = query.Include(h => h.Animal);
            }

            return await query
                .OrderByDescending(h => h.EventDate)
                .ToListAsync(cancellationToken);
        }
    }
}
