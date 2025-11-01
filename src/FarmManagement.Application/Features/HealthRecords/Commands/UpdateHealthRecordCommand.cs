using FarmManagement.Domain.Entities;
using FarmManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.HealthRecords.Commands
{
    public class UpdateHealthRecordCommand : IRequest<HealthRecord>
    {
        public int Id { get; set; }
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

    public class UpdateHealthRecordCommandHandler : IRequestHandler<UpdateHealthRecordCommand, HealthRecord>
    {
        private readonly IApplicationDbContext _context;

        public UpdateHealthRecordCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HealthRecord> Handle(UpdateHealthRecordCommand request, CancellationToken cancellationToken)
        {
            var healthRecord = await _context.HealthRecords
                .FirstOrDefaultAsync(h => h.Id == request.Id, cancellationToken);

            if (healthRecord == null)
            {
                throw new KeyNotFoundException($"HealthRecord with ID {request.Id} not found.");
            }

            healthRecord.AnimalId = request.AnimalId;
            healthRecord.EventType = request.EventType;
            healthRecord.EventDate = request.EventDate;
            healthRecord.Diagnosis = request.Diagnosis;
            healthRecord.Treatment = request.Treatment;
            healthRecord.Medication = request.Medication;
            healthRecord.AdministeredBy = request.AdministeredBy;
            healthRecord.NextDueDate = request.NextDueDate;
            healthRecord.Cost = request.Cost;
            healthRecord.Notes = request.Notes;

            await _context.SaveChangesAsync(cancellationToken);

            return healthRecord;
        }
    }
}
