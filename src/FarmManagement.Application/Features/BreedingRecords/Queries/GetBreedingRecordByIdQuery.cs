using FarmManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.BreedingRecords.Queries
{
    public class GetBreedingRecordByIdQuery : IRequest<BreedingRecord?>
    {
        public int Id { get; set; }
    }

    public class GetBreedingRecordByIdQueryHandler : IRequestHandler<GetBreedingRecordByIdQuery, BreedingRecord?>
    {
        private readonly IApplicationDbContext _context;

        public GetBreedingRecordByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BreedingRecord?> Handle(GetBreedingRecordByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.BreedingRecords
                .Include(b => b.Animal)
                .Include(b => b.Sire)
                .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);
        }
    }
}
