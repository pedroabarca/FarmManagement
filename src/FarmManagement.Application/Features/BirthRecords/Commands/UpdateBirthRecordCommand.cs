using FarmManagement.Domain.Entities;
using FarmManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.BirthRecords.Commands
{
    public class UpdateBirthRecordCommand : IRequest<BirthRecord>
    {
        public int Id { get; set; }
        public int DamId { get; set; }
        public int? CalfId { get; set; }
        public DateTime CalvingDate { get; set; }
        public CalvingEase CalvingEase { get; set; }
        public Sex CalfSex { get; set; }
        public decimal? CalfBirthWeightKg { get; set; }
        public CalfStatus CalfStatus { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateBirthRecordCommandHandler : IRequestHandler<UpdateBirthRecordCommand, BirthRecord>
    {
        private readonly IApplicationDbContext _context;

        public UpdateBirthRecordCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BirthRecord> Handle(UpdateBirthRecordCommand request, CancellationToken cancellationToken)
        {
            var birthRecord = await _context.BirthRecords
                .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            if (birthRecord == null)
            {
                throw new KeyNotFoundException($"BirthRecord with ID {request.Id} not found.");
            }

            birthRecord.DamId = request.DamId;
            birthRecord.CalfId = request.CalfId;
            birthRecord.CalvingDate = request.CalvingDate;
            birthRecord.CalvingEase = request.CalvingEase;
            birthRecord.CalfSex = request.CalfSex;
            birthRecord.CalfBirthWeightKg = request.CalfBirthWeightKg;
            birthRecord.CalfStatus = request.CalfStatus;
            birthRecord.Notes = request.Notes;

            await _context.SaveChangesAsync(cancellationToken);

            return birthRecord;
        }
    }
}
