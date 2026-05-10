using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Infrastructure;

/// <summary>
/// Keeps Program.cs clean by grouping all Infrastructure registrations here.
/// Add repositories, background services, etc. to this method as the project grows.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Data Source=taskmanager.db";

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(connectionString);

            // Logs generated SQL to the console in development.
            // Remove or replace with a proper logger in production.
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        });

        return services;
    }
}
