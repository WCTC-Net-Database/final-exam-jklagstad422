using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ConsoleRpgEntities.Helpers
{
    public static class ConfigurationHelper
    {
        public static IConfigurationRoot GetConfiguration(
            string basePath = null,
            string environmentName = null)
        {
            basePath ??= Directory.GetCurrentDirectory();

            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            if (!string.IsNullOrEmpty(environmentName))
            {
                builder.AddJsonFile(
                    $"appsettings.{environmentName}.json",
                    optional: true);
            }

            builder.AddEnvironmentVariables();

            return builder.Build();
        }

        // ✅ NO LAZY LOADING
        public static void ConfigureDbContextOptions(
            DbContextOptionsBuilder optionsBuilder,
            string connectionString)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
