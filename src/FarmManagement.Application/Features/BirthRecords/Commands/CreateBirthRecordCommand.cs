using FarmManagement.Domain.Entities;
using FarmManagement.Domain.Enums;
using MediatR;
using FarmManagement.Infrastructure.Persistence;

namespace FarmManagement.Application.Features.BirthRecords.Commands
{
    public class CreateBirthRecordCommand : IRequest<BirthRecord>
    {
        public int DamId { get; set; }
        public int? CalfId { get; set; }
        public DateTime CalvingDate { get; set; }
        public CalvingEase CalvingEase { get; set; } = CalvingEase.Easy;
        public Sex CalfSex { get; set; }
        public decimal? CalfBirthWeightKg { get; set; }
        public CalfStatus CalfStatus { get; set; } = CalfStatus.Alive;
        public string? Notes { get; set; }
    }

    public class CreateBirthRecordCommandHandler : IRequestHandler<CreateBirthRecordCommand, BirthRecord>
    {
        private readonly IApplicationDbContext _context;

        public CreateBirthRecordCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BirthRecord> Handle(CreateBirthRecordCommand request, CancellationToken cancellationToken)
        {
            var birthRecord = new BirthRecord
            {
                DamId = request.DamId,
                CalfId = request.CalfId,
                CalvingDate = request.CalvingDate,
                CalvingEase = request.CalvingEase,
                CalfSex = request.CalfSex,
                CalfBirthWeightKg = request.CalfBirthWeightKg,
                CalfStatus = request.CalfStatus,
                Notes = request.Notes
            };

            _context.BirthRecords.Add(birthRecord);
            await _context.SaveChangesAsync(cancellationToken);

            return birthRecord;
        }
    }
}
