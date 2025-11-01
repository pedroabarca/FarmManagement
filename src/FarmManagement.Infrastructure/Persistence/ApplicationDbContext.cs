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

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 📌 Aquí podemos agregar configuraciones de entidades personalizadas en el futuro.
        }
    }
}
