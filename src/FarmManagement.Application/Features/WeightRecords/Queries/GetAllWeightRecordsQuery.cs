using FarmManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.WeightRecords.Queries
{
    public class GetAllWeightRecordsQuery : IRequest<List<WeightRecord>>
    {
        public bool IncludeAnimal { get; set; } = false;
    }

    public class GetAllWeightRecordsQueryHandler : IRequestHandler<GetAllWeightRecordsQuery, List<WeightRecord>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllWeightRecordsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<WeightRecord>> Handle(GetAllWeightRecordsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.WeightRecords.AsQueryable();

            if (request.IncludeAnimal)
            {
                query = query.Include(w => w.Animal);
            }

            return await query
                .OrderByDescending(w => w.MeasurementDate)
                .ToListAsync(cancellationToken);
        }
    }
}
