using FarmManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.BirthRecords.Queries
{
    public class GetBirthRecordByIdQuery : IRequest<BirthRecord?>
    {
        public int Id { get; set; }
    }

    public class GetBirthRecordByIdQueryHandler : IRequestHandler<GetBirthRecordByIdQuery, BirthRecord?>
    {
        private readonly IApplicationDbContext _context;

        public GetBirthRecordByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BirthRecord?> Handle(GetBirthRecordByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.BirthRecords
                .Include(b => b.Dam)
                .Include(b => b.Calf)
                .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);
        }
    }
}
