using ConsoleRpg.Services;
using ConsoleRpgEntities.Data;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleRpg
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();
            Startup.ConfigureServices(services);

            var provider = services.BuildServiceProvider();

            using (var scope = provider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<GameContext>();
                GameSeeder.Seed(context); // 🔑 THIS LINE
            }

            var gameEngine = provider.GetRequiredService<GameEngine>();
            gameEngine.Start();
        }
    }
}
