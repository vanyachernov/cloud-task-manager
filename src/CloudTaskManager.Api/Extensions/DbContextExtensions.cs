using CloudTaskManager.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CloudTaskManager.Api.Extensions;

public static class DbContextExtensions
{
    public static IServiceCollection AddCloudTaskDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgresConnection");
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' is not set.");
        }

        services.AddDbContext<CloudTaskDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        return services;
    }
}