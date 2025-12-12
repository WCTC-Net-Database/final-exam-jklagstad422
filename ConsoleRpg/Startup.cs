using ConsoleRpg.Helpers;
using ConsoleRpg.Services;
using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NReco.Logging.File;

namespace ConsoleRpg;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        var configuration = ConfigurationHelper.GetConfiguration();

        // ---------------------------
        // Logging
        // ---------------------------
        var fileLoggerOptions = new FileLoggerOptions();
        configuration.GetSection("Logging:File").Bind(fileLoggerOptions);

        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddConfiguration(configuration.GetSection("Logging"));
            builder.AddConsole();

            // IMPORTANT: Logs folder must exist
            builder.AddProvider(
                new FileLoggerProvider("Logs/log.txt", fileLoggerOptions));
        });

        // ---------------------------
        // DbContext (NO MODELS YET)
        // ---------------------------
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<GameContext>(options =>
        {
            ConfigurationHelper.ConfigureDbContextOptions(options, connectionString);
        });

        // ---------------------------
        // Phase 1 Services ONLY
        // ---------------------------
        services.AddTransient<GameEngine>();
        services.AddSingleton<OutputManager>();
        // ---------------------------
        // Core helpers
        // ---------------------------
        services.AddSingleton<OutputManager>();
        services.AddSingleton<MenuManager>();

        // ---------------------------
        // UI / World
        // ---------------------------
        services.AddSingleton<MapManager>();
        services.AddSingleton<ExplorationUI>();

        // ---------------------------
        // Game services
        // ---------------------------
        services.AddTransient<PlayerService>();
        services.AddTransient<AdminService>();

        // ---------------------------
        // Engine
        // ---------------------------
        services.AddTransient<GameEngine>();

    }

}
