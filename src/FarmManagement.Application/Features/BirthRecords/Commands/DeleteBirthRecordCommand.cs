using MediatR;
using Microsoft.EntityFrameworkCore;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.BirthRecords.Commands
{
    public class DeleteBirthRecordCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteBirthRecordCommandHandler : IRequestHandler<DeleteBirthRecordCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public DeleteBirthRecordCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteBirthRecordCommand request, CancellationToken cancellationToken)
        {
            var birthRecord = await _context.BirthRecords
                .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            if (birthRecord == null)
            {
                return false;
            }

            _context.BirthRecords.Remove(birthRecord);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
