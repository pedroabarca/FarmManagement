using FarmManagement.Domain.Entities;
using FarmManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.WeightRecords.Commands
{
    public class UpdateWeightRecordCommand : IRequest<WeightRecord>
    {
        public int Id { get; set; }
        public int AnimalId { get; set; }
        public decimal WeightKg { get; set; }
        public DateTime MeasurementDate { get; set; }
        public WeightMeasurementType MeasurementType { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateWeightRecordCommandHandler : IRequestHandler<UpdateWeightRecordCommand, WeightRecord>
    {
        private readonly IApplicationDbContext _context;

        public UpdateWeightRecordCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WeightRecord> Handle(UpdateWeightRecordCommand request, CancellationToken cancellationToken)
        {
            var weightRecord = await _context.WeightRecords
                .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

            if (weightRecord == null)
            {
                throw new KeyNotFoundException($"WeightRecord with ID {request.Id} not found.");
            }

            weightRecord.AnimalId = request.AnimalId;
            weightRecord.WeightKg = request.WeightKg;
            weightRecord.MeasurementDate = request.MeasurementDate;
            weightRecord.MeasurementType = request.MeasurementType;
            weightRecord.Notes = request.Notes;

            await _context.SaveChangesAsync(cancellationToken);

            return weightRecord;
        }
    }
}
