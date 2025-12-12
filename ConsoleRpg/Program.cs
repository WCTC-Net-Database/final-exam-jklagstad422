using ConsoleRpg;
using ConsoleRpg.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleRpg;

public class Program
{
    public static void Main(string[] args)
    {
        var services = new ServiceCollection();
        Startup.ConfigureServices(services);

        var provider = services.BuildServiceProvider();

        var gameEngine = provider.GetRequiredService<GameEngine>();

        gameEngine.Start(); // 🔑 THIS MUST RUN
    }
}
