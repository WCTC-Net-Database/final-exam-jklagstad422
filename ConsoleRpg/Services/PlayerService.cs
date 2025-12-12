using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Characters;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace ConsoleRpg.Services;

public class PlayerService
{
    private readonly GameContext _ctx;

    public PlayerService(GameContext ctx)
    {
        _ctx = ctx;
    }

    public void Attack(Player inputPlayer)
    {
        // 🔑 ALWAYS reload tracked entities
        var player = _ctx.Players
            .FirstOrDefault(p => p.Id == inputPlayer.Id);

        if (player == null || player.RoomId == null)
            return;

        var monster = _ctx.Monsters
            .FirstOrDefault(m => m.RoomId == player.RoomId);

        if (monster == null)
        {
            AnsiConsole.MarkupLine("[yellow]No monsters here[/]");
            return;
        }

        monster.Health -= player.Attack;

        if (monster.Health <= 0)
            _ctx.Monsters.Remove(monster);

        _ctx.SaveChanges(); // ✅ THIS WAS MISSING BEFORE

        AnsiConsole.MarkupLine($"[green]You hit {monster.Name}![/]");
    }
}
