using FarmManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.Animals.Queries
{
    public class GetAllAnimalsQuery : IRequest<List<Animal>> { }

    public class GetAllAnimalsQueryHandler : IRequestHandler<GetAllAnimalsQuery, List<Animal>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllAnimalsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Animal>> Handle(GetAllAnimalsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Animals.ToListAsync(cancellationToken);
        }
    }
}
