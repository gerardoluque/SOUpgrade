using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SOUpgrade.Domain.Interfaces;
using SOUpgrade.Infrastructure.Persistence;
using SOUpgrade.Infrastructure.Repositories;

namespace SOUpgrade.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
            ));

        services.AddScoped<IServiceOrderRepository, ServiceOrderRepository>();

        return services;
    }
}
