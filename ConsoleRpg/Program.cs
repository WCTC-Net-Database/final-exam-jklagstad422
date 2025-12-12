using ConsoleRpg;
using ConsoleRpg.Services;
using ConsoleRpgEntities.Data;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    private static void Main()
    {
        // -----------------------------
        // Configure DI
        // -----------------------------
        var services = new ServiceCollection();
        Startup.ConfigureServices(services);

        using var provider = services.BuildServiceProvider();

        // -----------------------------
        // ONE-TIME DATABASE SETUP
        // -----------------------------
        using (var scope = provider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GameContext>();

   

            // 🔑 Seed base rooms safely
            GameSeeder.Seed(context);
        }

        // -----------------------------
        // START GAME
        // -----------------------------
        var engine = provider.GetRequiredService<GameEngine>();
        engine.Start();
    }
}
