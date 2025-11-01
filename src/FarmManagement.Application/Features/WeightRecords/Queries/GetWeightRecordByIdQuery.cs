using FarmManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.WeightRecords.Queries
{
    public class GetWeightRecordByIdQuery : IRequest<WeightRecord?>
    {
        public int Id { get; set; }
    }

    public class GetWeightRecordByIdQueryHandler : IRequestHandler<GetWeightRecordByIdQuery, WeightRecord?>
    {
        private readonly IApplicationDbContext _context;

        public GetWeightRecordByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WeightRecord?> Handle(GetWeightRecordByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.WeightRecords
                .Include(w => w.Animal)
                .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);
        }
    }
}
