using MediatR;
using Microsoft.EntityFrameworkCore;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.HealthRecords.Commands
{
    public class DeleteHealthRecordCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteHealthRecordCommandHandler : IRequestHandler<DeleteHealthRecordCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public DeleteHealthRecordCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteHealthRecordCommand request, CancellationToken cancellationToken)
        {
            var healthRecord = await _context.HealthRecords
                .FirstOrDefaultAsync(h => h.Id == request.Id, cancellationToken);

            if (healthRecord == null)
            {
                return false;
            }

            _context.HealthRecords.Remove(healthRecord);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
