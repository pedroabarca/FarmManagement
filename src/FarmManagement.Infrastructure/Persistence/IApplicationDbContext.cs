using FarmManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FarmManagement.Infrastructure.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<Animal> Animals { get; }
        DbSet<Employee> Employees { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
