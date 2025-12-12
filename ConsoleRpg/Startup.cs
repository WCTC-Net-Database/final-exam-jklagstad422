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
        var config = ConfigurationHelper.GetConfiguration();

        services.AddLogging(b =>
        {
            b.ClearProviders();
            b.AddConsole();
            b.AddProvider(new FileLoggerProvider("Logs/log.txt", new FileLoggerOptions()));
        });

        services.AddDbContext<GameContext>(options =>
            ConfigurationHelper.ConfigureDbContextOptions(
                options,
                config.GetConnectionString("DefaultConnection")));

        services.AddSingleton<MenuManager>();
        services.AddSingleton<MapManager>();
        services.AddSingleton<ExplorationUI>();

        services.AddScoped<PlayerService>();
        services.AddScoped<AdminService>();
        services.AddScoped<GameEngine>();
    }
}
