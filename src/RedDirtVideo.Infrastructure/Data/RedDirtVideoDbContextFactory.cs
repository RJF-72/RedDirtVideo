using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using RedDirtVideo.Infrastructure.Data;

namespace RedDirtVideo.Infrastructure.Data;

public class RedDirtVideoDbContextFactory : IDesignTimeDbContextFactory<RedDirtVideoDbContext>
{
    public RedDirtVideoDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Host=localhost;Port=5432;Database=RedDirtVideoDb;Username=postgres;Password=postgres;";

        var optionsBuilder = new DbContextOptionsBuilder<RedDirtVideoDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new RedDirtVideoDbContext(optionsBuilder.Options);
    }
}
