using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Rooms;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace ConsoleRpg.Services;

public class AdminService
{
    private readonly GameContext _ctx;

    public AdminService(GameContext ctx)
    {
        _ctx = ctx;
    }

    public void AddCharacter()
    {
        var p = new Player
        {
            Name = AnsiConsole.Ask<string>("Name"),
            Health = AnsiConsole.Ask<int>("Health"),
            Attack = AnsiConsole.Ask<int>("Attack"),
            Experience = 0,
            RoomId = _ctx.Rooms.FirstOrDefault()?.Id
        };

        _ctx.Players.Add(p);
        _ctx.SaveChanges();
    }

    public void EditCharacter()
    {
        var id = AnsiConsole.Ask<int>("ID");
        var p = _ctx.Players.Find(id);
        if (p == null) return;


        p.Health = AnsiConsole.Ask<int>("New Health");
        p.Attack = AnsiConsole.Ask<int>("New Attack");
        _ctx.SaveChanges();
    }

    public void DisplayAllCharacters()
    {
        foreach (var p in _ctx.Players.Include(p => p.Room))
            AnsiConsole.MarkupLine($"{p.Id} {p.Name} HP:{p.Health} Room:{p.Room?.Name}");
        Console.ReadKey();
    }

    public void SearchCharacterByName()
    {
        var q = AnsiConsole.Ask<string>("Search");
        foreach (var p in _ctx.Players.Where(p => p.Name.Contains(q)))
            AnsiConsole.MarkupLine(p.Name);
        Console.ReadKey();
    }

    public void AddRoom()
    {
        _ctx.Rooms.Add(new Room
        {
            Name = AnsiConsole.Ask<string>("Room name"),
            Description = AnsiConsole.Ask<string>("Description")
        });
        _ctx.SaveChanges();
    }

    public void DisplayAllRooms()
    {
        foreach (var r in _ctx.Rooms.Include(r => r.Monsters))
            AnsiConsole.MarkupLine($"{r.Id} {r.Name} Monsters:{r.Monsters.Count}");
        Console.ReadKey();
    }
}
