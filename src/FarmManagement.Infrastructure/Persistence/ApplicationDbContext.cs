using FarmManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FarmManagement.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Animal> Animals => Set<Animal>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<WeightRecord> WeightRecords => Set<WeightRecord>();
        public DbSet<BirthRecord> BirthRecords => Set<BirthRecord>();
        public DbSet<BreedingRecord> BreedingRecords => Set<BreedingRecord>();
        public DbSet<HealthRecord> HealthRecords => Set<HealthRecord>();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Animal Entity Configuration
            modelBuilder.Entity<Animal>(entity =>
            {
                entity.HasKey(a => a.Id);

                // TagId must be unique
                entity.HasIndex(a => a.TagId).IsUnique();

                // ElectronicId can be unique but nullable
                entity.HasIndex(a => a.ElectronicId).IsUnique();

                // Self-referencing relationships for genealogy (Sire and Dam)
                entity.HasOne(a => a.Sire)
                    .WithMany()
                    .HasForeignKey(a => a.SireId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Dam)
                    .WithMany()
                    .HasForeignKey(a => a.DamId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relationships with related tables
                entity.HasMany(a => a.WeightRecords)
                    .WithOne(w => w.Animal)
                    .HasForeignKey(w => w.AnimalId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(a => a.BirthRecordsAsMother)
                    .WithOne(b => b.Dam)
                    .HasForeignKey(b => b.DamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(a => a.BreedingRecords)
                    .WithOne(b => b.Animal)
                    .HasForeignKey(b => b.AnimalId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(a => a.HealthRecords)
                    .WithOne(h => h.Animal)
                    .HasForeignKey(h => h.AnimalId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // WeightRecord Entity Configuration
            modelBuilder.Entity<WeightRecord>(entity =>
            {
                entity.HasKey(w => w.Id);
                entity.HasIndex(w => new { w.AnimalId, w.MeasurementDate });
            });

            // BirthRecord Entity Configuration
            modelBuilder.Entity<BirthRecord>(entity =>
            {
                entity.HasKey(b => b.Id);

                // Relationship with the calf (optional, can be null if calf didn't survive)
                entity.HasOne(b => b.Calf)
                    .WithMany()
                    .HasForeignKey(b => b.CalfId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // BreedingRecord Entity Configuration
            modelBuilder.Entity<BreedingRecord>(entity =>
            {
                entity.HasKey(b => b.Id);

                // Relationship with Sire (bull used for breeding)
                entity.HasOne(b => b.Sire)
                    .WithMany()
                    .HasForeignKey(b => b.SireId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(b => new { b.AnimalId, b.EventDate });
            });

            // HealthRecord Entity Configuration
            modelBuilder.Entity<HealthRecord>(entity =>
            {
                entity.HasKey(h => h.Id);
                entity.HasIndex(h => new { h.AnimalId, h.EventDate });
            });
        }
    }
}
