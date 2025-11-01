using FarmManagement.Domain.Enums;

namespace FarmManagement.Domain.Entities
{
    public class WeightRecord
    {
        public int Id { get; set; }
        public int AnimalId { get; set; }
        public Animal Animal { get; set; } = null!;

        public decimal WeightKg { get; set; }
        public DateTime MeasurementDate { get; set; }
        public WeightMeasurementType MeasurementType { get; set; } = WeightMeasurementType.Regular;
        public string? Notes { get; set; }
    }
}
