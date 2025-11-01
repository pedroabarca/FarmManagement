using MediatR;
using Microsoft.EntityFrameworkCore;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.Animals.Commands
{
    public class DeleteAnimalCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteAnimalCommandHandler : IRequestHandler<DeleteAnimalCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public DeleteAnimalCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteAnimalCommand request, CancellationToken cancellationToken)
        {
            var animal = await _context.Animals
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (animal == null)
            {
                return false;
            }

            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
