
using AVPermitSystemV2.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AVPermitSystemV2.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DB Context
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // Add other services here

        return services;
    }
}
