using FarmManagement.Domain.Enums;

namespace FarmManagement.Domain.Entities
{
    public class Animal
    {
        // Identity
        public int Id { get; set; }
        public string TagId { get; set; } = string.Empty;  // Physical ear tag (unique identifier)
        public string? ElectronicId { get; set; }           // RFID/Electronic tag (optional)
        public string? Name { get; set; }                   // Optional friendly name

        // Basic Info
        public string Breed { get; set; } = string.Empty;   // e.g., "Holstein", "Angus"
        public Sex Sex { get; set; }
        public DateTime BirthDate { get; set; }
        public decimal? BirthWeightKg { get; set; }
        public decimal? WeaningWeightKg { get; set; }
        public DateTime? WeaningDate { get; set; }

        // Genealogy
        public int? SireId { get; set; }                    // FK - Father
        public Animal? Sire { get; set; }
        public int? DamId { get; set; }                     // FK - Mother
        public Animal? Dam { get; set; }

        // Reproductive tracking (denormalized for quick access)
        public DateTime? LastCalvingDate { get; set; }
        public DateTime? LastHeatDate { get; set; }
        public DateTime? LastBreedingDate { get; set; }
        public DateTime? LastPalpationDate { get; set; }
        public DateTime? NextExpectedCalvingDate { get; set; }

        // Management
        public AnimalStatus Status { get; set; } = AnimalStatus.Active;
        public string? CurrentLocation { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public decimal? PurchasePrice { get; set; }

        // Metadata
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<WeightRecord> WeightRecords { get; set; } = new List<WeightRecord>();
        public ICollection<BirthRecord> BirthRecordsAsMother { get; set; } = new List<BirthRecord>();
        public ICollection<BreedingRecord> BreedingRecords { get; set; } = new List<BreedingRecord>();
        public ICollection<HealthRecord> HealthRecords { get; set; } = new List<HealthRecord>();

        // Kept for backward compatibility
        [Obsolete("Use Breed instead")]
        public string Species
        {
            get => Breed;
            set => Breed = value;
        }

        [Obsolete("Use BirthDate instead")]
        public DateTime DateOfBirth
        {
            get => BirthDate;
            set => BirthDate = value;
        }
    }
}
