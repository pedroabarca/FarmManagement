using FarmManagement.Domain.Entities;
using FarmManagement.Domain.Enums;
using MediatR;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.HealthRecords.Commands
{
    public class CreateHealthRecordCommand : IRequest<HealthRecord>
    {
        public int AnimalId { get; set; }
        public HealthEventType EventType { get; set; }
        public DateTime EventDate { get; set; }
        public string? Diagnosis { get; set; }
        public string? Treatment { get; set; }
        public string? Medication { get; set; }
        public string? AdministeredBy { get; set; }
        public DateTime? NextDueDate { get; set; }
        public decimal? Cost { get; set; }
        public string? Notes { get; set; }
    }

    public class CreateHealthRecordCommandHandler : IRequestHandler<CreateHealthRecordCommand, HealthRecord>
    {
        private readonly IApplicationDbContext _context;

        public CreateHealthRecordCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HealthRecord> Handle(CreateHealthRecordCommand request, CancellationToken cancellationToken)
        {
            var healthRecord = new HealthRecord
            {
                AnimalId = request.AnimalId,
                EventType = request.EventType,
                EventDate = request.EventDate,
                Diagnosis = request.Diagnosis,
                Treatment = request.Treatment,
                Medication = request.Medication,
                AdministeredBy = request.AdministeredBy,
                NextDueDate = request.NextDueDate,
                Cost = request.Cost,
                Notes = request.Notes
            };

            _context.HealthRecords.Add(healthRecord);
            await _context.SaveChangesAsync(cancellationToken);

            return healthRecord;
        }
    }
}
