using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MobyLabWebProgramming.Infrastructure.Database;

/// <summary>
/// This factory is used to create instances of WebAppDatabaseContext at design time for migrations and other tooling operations.
/// </summary>
public sealed class WebAppDatabaseContextFactory : IDesignTimeDbContextFactory<WebAppDatabaseContext>
{
    public WebAppDatabaseContext CreateDbContext(string[] args)
    {
        var basePath = FindBasePath();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        var optionsBuilder = new DbContextOptionsBuilder<WebAppDatabaseContext>();
        optionsBuilder.UseNpgsql(
            configuration.GetConnectionString("WebAppDatabase"),
            o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)
                .CommandTimeout((int)TimeSpan.FromMinutes(15).TotalSeconds));

        return new WebAppDatabaseContext((DbContextOptions<WebAppDatabaseContext>)optionsBuilder.Options, migrate: false);
    }

    private static string FindBasePath()
    {
        // Start from current directory and walk up to find the solution root
        var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (currentDirectory != null)
        {
            // Check if Backend folder exists at this level
            var backendPath = Path.Combine(currentDirectory.FullName, "MobyLabWebProgramming.Backend");
            var appSettingsPath = Path.Combine(backendPath, "appsettings.json");

            // Verify both the directory and file exist
            if (Directory.Exists(backendPath) && File.Exists(appSettingsPath))
            {
                return backendPath;
            }

            // Move up one directory level
            currentDirectory = currentDirectory.Parent;
        }

        throw new InvalidOperationException("Could not find MobyLabWebProgramming.Backend folder with appsettings.json");
    }
}
