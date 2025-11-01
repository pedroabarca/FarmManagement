using MediatR;
using Microsoft.EntityFrameworkCore;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.WeightRecords.Commands
{
    public class DeleteWeightRecordCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteWeightRecordCommandHandler : IRequestHandler<DeleteWeightRecordCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public DeleteWeightRecordCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteWeightRecordCommand request, CancellationToken cancellationToken)
        {
            var weightRecord = await _context.WeightRecords
                .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

            if (weightRecord == null)
            {
                return false;
            }

            _context.WeightRecords.Remove(weightRecord);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
