using FarmManagement.Domain.Entities;
using FarmManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.BreedingRecords.Commands
{
    public class UpdateBreedingRecordCommand : IRequest<BreedingRecord>
    {
        public int Id { get; set; }
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

    public class UpdateBreedingRecordCommandHandler : IRequestHandler<UpdateBreedingRecordCommand, BreedingRecord>
    {
        private readonly IApplicationDbContext _context;

        public UpdateBreedingRecordCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BreedingRecord> Handle(UpdateBreedingRecordCommand request, CancellationToken cancellationToken)
        {
            var breedingRecord = await _context.BreedingRecords
                .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            if (breedingRecord == null)
            {
                throw new KeyNotFoundException($"BreedingRecord with ID {request.Id} not found.");
            }

            breedingRecord.AnimalId = request.AnimalId;
            breedingRecord.EventType = request.EventType;
            breedingRecord.EventDate = request.EventDate;
            breedingRecord.SireId = request.SireId;
            breedingRecord.BreedingMethod = request.BreedingMethod;
            breedingRecord.PregnancyStatus = request.PregnancyStatus;
            breedingRecord.ExpectedCalvingDate = request.ExpectedCalvingDate;
            breedingRecord.TechnicianName = request.TechnicianName;
            breedingRecord.Notes = request.Notes;

            await _context.SaveChangesAsync(cancellationToken);

            return breedingRecord;
        }
    }
}
