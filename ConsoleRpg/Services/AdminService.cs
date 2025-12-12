using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace ConsoleRpg.Services;

public class AdminService
{
    private readonly GameContext _context;
    private readonly ILogger<AdminService> _logger;

    public AdminService(GameContext context, ILogger<AdminService> logger)
    {
        _context = context;
        _logger = logger;
    }

    // =====================================================
    // BASIC CRUD
    // =====================================================
    public void AddCharacter()
    {
        try
        {
            AnsiConsole.MarkupLine("[yellow]=== Add New Character ===[/]");

            var name = AnsiConsole.Ask<string>("Enter [green]name[/]:");
            var health = AnsiConsole.Ask<int>("Enter [green]health[/]:");
            var experience = AnsiConsole.Ask<int>("Enter [green]experience[/]:");

            var player = new Player
            {
                Name = name,
                Health = health,
                Experience = experience
            };

            _context.Players.Add(player);
            _context.SaveChanges();

            // Ensure player starts in a room
            var firstRoom = _context.Rooms.FirstOrDefault();
            if (firstRoom != null)
            {
                player.RoomId = firstRoom.Id;
                _context.SaveChanges();
            }

            AnsiConsole.MarkupLine($"[green]Character '{name}' created successfully![/]");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding character");
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
            Pause();
        }
    }

    public void EditCharacter()
    {
        var id = AnsiConsole.Ask<int>("Enter character ID:");
        var player = _context.Players.Find(id);

        if (player == null)
        {
            AnsiConsole.MarkupLine("[red]Character not found[/]");
            return;
        }

        if (AnsiConsole.Confirm("Update name?"))
            player.Name = AnsiConsole.Ask<string>("New name:");

        if (AnsiConsole.Confirm("Update health?"))
            player.Health = AnsiConsole.Ask<int>("New health:");

        if (AnsiConsole.Confirm("Update experience?"))
            player.Experience = AnsiConsole.Ask<int>("New experience:");

        _context.SaveChanges();
        AnsiConsole.MarkupLine("[green]Character updated[/]");
    }

    public void DisplayAllCharacters()
    {
        var players = _context.Players.Include(p => p.Room).ToList();

        if (!players.Any())
        {
            AnsiConsole.MarkupLine("[red]No characters found[/]");
            return;
        }

        var table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Name");
        table.AddColumn("Health");
        table.AddColumn("XP");
        table.AddColumn("Room");

        foreach (var p in players)
        {
            table.AddRow(
                p.Id.ToString(),
                p.Name,
                p.Health.ToString(),
                p.Experience.ToString(),
                p.Room?.Name ?? "None");
        }

        AnsiConsole.Write(table);
    }

    public void SearchCharacterByName()
    {
        var search = AnsiConsole.Ask<string>("Search name:");

        var results = _context.Players
            .Include(p => p.Room)
            .Where(p => p.Name.ToLower().Contains(search.ToLower()))
            .ToList();

        if (!results.Any())
        {
            AnsiConsole.MarkupLine("[red]No matches found[/]");
            return;
        }

        foreach (var p in results)
        {
            AnsiConsole.MarkupLine(
                $"[green]{p.Name}[/] | HP: {p.Health} | Room: {p.Room?.Name ?? "None"}");
        }
    }

    // =====================================================
    // B-LEVEL REQUIRED FEATURE
    // =====================================================
    public void AddRoom()
    {
        try
        {
            AnsiConsole.MarkupLine("[yellow]=== Add New Room ===[/]");

            var name = AnsiConsole.Ask<string>("Room name:");
            var description = AnsiConsole.Ask<string>("Room description:");

            var room = new Room
            {
                Name = name,
                Description = description
            };

            _context.Rooms.Add(room);
            _context.SaveChanges();

            AnsiConsole.MarkupLine($"[green]Room '{room.Name}' created (ID {room.Id})[/]");

            // Optional connection
            if (_context.Rooms.Count() > 1 &&
                AnsiConsole.Confirm("Connect this room to an existing room?"))
            {
                var rooms = _context.Rooms.ToList();

                var table = new Table();
                table.AddColumn("ID");
                table.AddColumn("Name");

                foreach (var r in rooms)
                    table.AddRow(r.Id.ToString(), r.Name);

                AnsiConsole.Write(table);

                var targetId = AnsiConsole.Ask<int>("Enter room ID to connect to:");
                var target = _context.Rooms.Find(targetId);

                if (target != null)
                {
                    var direction = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Direction from existing room")
                            .AddChoices("North", "South", "East", "West"));

                    switch (direction)
                    {
                        case "North":
                            target.NorthRoomId = room.Id;
                            room.SouthRoomId = target.Id;
                            break;
                        case "South":
                            target.SouthRoomId = room.Id;
                            room.NorthRoomId = target.Id;
                            break;
                        case "East":
                            target.EastRoomId = room.Id;
                            room.WestRoomId = target.Id;
                            break;
                        case "West":
                            target.WestRoomId = room.Id;
                            room.EastRoomId = target.Id;
                            break;
                    }

                    _context.SaveChanges();
                    AnsiConsole.MarkupLine("[green]Rooms connected[/]");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding room");
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
            Pause();
        }
    }

    // =====================================================
    // PLACEHOLDERS (NOT REQUIRED FOR B)
    // =====================================================
    public void AddAbilityToCharacter()
    {
        AnsiConsole.MarkupLine("[yellow]Ability management not implemented[/]");
        Pause();
    }

    public void DisplayCharacterAbilities()
    {
        AnsiConsole.MarkupLine("[yellow]Ability display not implemented[/]");
        Pause();
    }

    public void DisplayRoomDetails()
    {
        AnsiConsole.MarkupLine("[yellow]Room details not implemented[/]");
        Pause();
    }

    // =====================================================
    // HELPERS
    // =====================================================
    private void Pause()
    {
        AnsiConsole.Markup("[dim]Press any key to continue...[/]");
        Console.ReadKey(true);
    }
}
