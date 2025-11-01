using FarmManagement.Domain.Entities;
using FarmManagement.Domain.Enums;
using MediatR;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.BreedingRecords.Commands
{
    public class CreateBreedingRecordCommand : IRequest<BreedingRecord>
    {
        public int AnimalId { get; set; }
        public BreedingEventType EventType { get; set; }
        public DateTime EventDate { get; set; }
        public int? SireId { get; set; }
        public BreedingMethod? BreedingMethod { get; set; }
        public PregnancyStatus? PregnancyStatus { get; set; }
        public DateTime? ExpectedCalvingDate { get; set; }
        public string? TechnicianName { get; set; }
        public string? Notes { get; set; }
    }

    public class CreateBreedingRecordCommandHandler : IRequestHandler<CreateBreedingRecordCommand, BreedingRecord>
    {
        private readonly IApplicationDbContext _context;

        public CreateBreedingRecordCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BreedingRecord> Handle(CreateBreedingRecordCommand request, CancellationToken cancellationToken)
        {
            var breedingRecord = new BreedingRecord
            {
                AnimalId = request.AnimalId,
                EventType = request.EventType,
                EventDate = request.EventDate,
                SireId = request.SireId,
                BreedingMethod = request.BreedingMethod,
                PregnancyStatus = request.PregnancyStatus,
                ExpectedCalvingDate = request.ExpectedCalvingDate,
                TechnicianName = request.TechnicianName,
                Notes = request.Notes
            };

            _context.BreedingRecords.Add(breedingRecord);
            await _context.SaveChangesAsync(cancellationToken);

            return breedingRecord;
        }
    }
}
