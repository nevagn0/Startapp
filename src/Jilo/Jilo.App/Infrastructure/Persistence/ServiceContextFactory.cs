using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Jilo.App.Infrastructure.Persistence; // ваше пространство имён

public class ServiceContextFactory : IDesignTimeDbContextFactory<ServiceContext>
{
    public ServiceContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ServiceContext>();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var connectionString = configuration.GetConnectionString("PostgresDatabase");
        optionsBuilder.UseNpgsql(connectionString); // или UseNpgsql и т.д.

        return new ServiceContext(optionsBuilder.Options);
    }
}