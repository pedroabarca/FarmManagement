using MediatR;
using Microsoft.EntityFrameworkCore;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.BreedingRecords.Commands
{
    public class DeleteBreedingRecordCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteBreedingRecordCommandHandler : IRequestHandler<DeleteBreedingRecordCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public DeleteBreedingRecordCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteBreedingRecordCommand request, CancellationToken cancellationToken)
        {
            var breedingRecord = await _context.BreedingRecords
                .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            if (breedingRecord == null)
            {
                return false;
            }

            _context.BreedingRecords.Remove(breedingRecord);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
