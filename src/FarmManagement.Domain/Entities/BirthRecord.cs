using FarmManagement.Domain.Enums;

namespace FarmManagement.Domain.Entities
{
    public class BirthRecord
    {
        public int Id { get; set; }
        public int DamId { get; set; }                       // FK - The mother
        public Animal Dam { get; set; } = null!;
        public int? CalfId { get; set; }                     // FK - The calf (null if didn't survive)
        public Animal? Calf { get; set; }

        public DateTime CalvingDate { get; set; }
        public CalvingEase CalvingEase { get; set; } = CalvingEase.Easy;
        public Sex CalfSex { get; set; }
        public decimal? CalfBirthWeightKg { get; set; }
        public CalfStatus CalfStatus { get; set; } = CalfStatus.Alive;
        public string? Notes { get; set; }
    }
}
