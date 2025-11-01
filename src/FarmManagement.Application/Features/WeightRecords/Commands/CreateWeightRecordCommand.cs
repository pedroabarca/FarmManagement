using FarmManagement.Domain.Entities;
using FarmManagement.Domain.Enums;
using MediatR;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.WeightRecords.Commands
{
    public class CreateWeightRecordCommand : IRequest<WeightRecord>
    {
        public int AnimalId { get; set; }
        public decimal WeightKg { get; set; }
        public DateTime MeasurementDate { get; set; }
        public WeightMeasurementType MeasurementType { get; set; } = WeightMeasurementType.Regular;
        public string? Notes { get; set; }
    }

    public class CreateWeightRecordCommandHandler : IRequestHandler<CreateWeightRecordCommand, WeightRecord>
    {
        private readonly IApplicationDbContext _context;

        public CreateWeightRecordCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WeightRecord> Handle(CreateWeightRecordCommand request, CancellationToken cancellationToken)
        {
            var weightRecord = new WeightRecord
            {
                AnimalId = request.AnimalId,
                WeightKg = request.WeightKg,
                MeasurementDate = request.MeasurementDate,
                MeasurementType = request.MeasurementType,
                Notes = request.Notes
            };

            _context.WeightRecords.Add(weightRecord);
            await _context.SaveChangesAsync(cancellationToken);

            return weightRecord;
        }
    }
}
