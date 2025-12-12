using ConsoleRpg;
using ConsoleRpg.Services;
using ConsoleRpgEntities.Data;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main()
    {
        var services = new ServiceCollection();
        Startup.ConfigureServices(services);

        using var provider = services.BuildServiceProvider();

        // 🔑 SEED ONCE, SAFELY
        using (var scope = provider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GameContext>();
            GameSeeder.Seed(context);
        }

        var engine = provider.GetRequiredService<GameEngine>();
        engine.Start();
    }
}
