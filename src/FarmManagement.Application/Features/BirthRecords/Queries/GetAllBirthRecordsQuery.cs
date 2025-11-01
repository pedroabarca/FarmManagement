using FarmManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.BirthRecords.Queries
{
    public class GetAllBirthRecordsQuery : IRequest<List<BirthRecord>>
    {
        public bool IncludeRelatedData { get; set; } = false;
    }

    public class GetAllBirthRecordsQueryHandler : IRequestHandler<GetAllBirthRecordsQuery, List<BirthRecord>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllBirthRecordsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BirthRecord>> Handle(GetAllBirthRecordsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.BirthRecords.AsQueryable();

            if (request.IncludeRelatedData)
            {
                query = query
                    .Include(b => b.Dam)
                    .Include(b => b.Calf);
            }

            return await query
                .OrderByDescending(b => b.CalvingDate)
                .ToListAsync(cancellationToken);
        }
    }
}
