using FarmManagement.Domain.Enums;

namespace FarmManagement.Domain.Entities
{
    public class BreedingRecord
    {
        public int Id { get; set; }
        public int AnimalId { get; set; }                    // FK - The cow
        public Animal Animal { get; set; } = null!;

        public BreedingEventType EventType { get; set; }
        public DateTime EventDate { get; set; }

        // Breeding-specific fields
        public int? SireId { get; set; }                     // FK - Bull used (if breeding)
        public Animal? Sire { get; set; }
        public BreedingMethod? BreedingMethod { get; set; }

        // Pregnancy check fields
        public PregnancyStatus? PregnancyStatus { get; set; }
        public DateTime? ExpectedCalvingDate { get; set; }

        public string? TechnicianName { get; set; }
        public string? Notes { get; set; }
    }
}
