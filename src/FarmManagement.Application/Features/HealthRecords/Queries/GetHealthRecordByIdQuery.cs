using FarmManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.HealthRecords.Queries
{
    public class GetHealthRecordByIdQuery : IRequest<HealthRecord?>
    {
        public int Id { get; set; }
    }

    public class GetHealthRecordByIdQueryHandler : IRequestHandler<GetHealthRecordByIdQuery, HealthRecord?>
    {
        private readonly IApplicationDbContext _context;

        public GetHealthRecordByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HealthRecord?> Handle(GetHealthRecordByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.HealthRecords
                .Include(h => h.Animal)
                .FirstOrDefaultAsync(h => h.Id == request.Id, cancellationToken);
        }
    }
}
