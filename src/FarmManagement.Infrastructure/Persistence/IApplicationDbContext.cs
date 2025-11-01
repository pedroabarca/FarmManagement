using FarmManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FarmManagement.Infrastructure.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<Animal> Animals { get; }
        DbSet<Employee> Employees { get; }
        DbSet<WeightRecord> WeightRecords { get; }
        DbSet<BirthRecord> BirthRecords { get; }
        DbSet<BreedingRecord> BreedingRecords { get; }
        DbSet<HealthRecord> HealthRecords { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
