namespace FarmManagement.Domain.Enums
{
    public enum Sex
    {
        Male,
        Female,
        Steer
    }

    public enum AnimalStatus
    {
        Active,
        Sold,
        Dead,
        Quarantine
    }

    public enum WeightMeasurementType
    {
        Regular,
        PreSale,
        PostTreatment,
        PreBreeding,
        Other
    }

    public enum CalvingEase
    {
        Easy,
        Difficult,
        Assisted,
        Cesarean
    }

    public enum CalfStatus
    {
        Alive,
        Stillborn,
        DiedAfterBirth
    }

    public enum BreedingEventType
    {
        Heat,               // Observed in heat
        Breeding,           // Bred (natural or AI)
        Palpation,         // Pregnancy check
        PregnancyCheck,    // Ultrasound/blood test
        DryingOff          // Stopped milking before calving
    }

    public enum BreedingMethod
    {
        Natural,
        ArtificialInsemination
    }

    public enum PregnancyStatus
    {
        Pregnant,
        Open,
        Uncertain
    }

    public enum HealthEventType
    {
        Vaccination,
        Treatment,
        Injury,
        Surgery,
        Checkup
    }
}
