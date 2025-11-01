using FarmManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FarmManagement.Infrastructure
{
    public static class InfrastructureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 📌 Verificar que DatabaseProvider esté configurado
            var databaseProvider = configuration.GetSection("DatabaseProvider").Value ?? "SqlServer";

            if (databaseProvider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            }
            else if (databaseProvider.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            }
            else
            {
                throw new Exception($"Database provider '{databaseProvider}' is not supported.");
            }

            // 📌 Registrar IApplicationDbContext
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            return services;
        }
    }
}
