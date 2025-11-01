using FarmManagement.Domain.Enums;

namespace FarmManagement.Domain.Entities
{
    public class HealthRecord
    {
        public int Id { get; set; }
        public int AnimalId { get; set; }
        public Animal Animal { get; set; } = null!;

        public HealthEventType EventType { get; set; }
        public DateTime EventDate { get; set; }

        public string? Diagnosis { get; set; }
        public string? Treatment { get; set; }
        public string? Medication { get; set; }
        public string? AdministeredBy { get; set; }          // e.g., "Carlos", "Dr. Rodriguez"

        public DateTime? NextDueDate { get; set; }           // For vaccination reminders
        public decimal? Cost { get; set; }
        public string? Notes { get; set; }
    }
}
