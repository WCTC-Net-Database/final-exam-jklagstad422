using ConsoleRpg.Helpers;
using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Rooms;
using Microsoft.EntityFrameworkCore;

namespace ConsoleRpg.Services;

public class GameEngine
{
    private readonly GameContext _context;
    private readonly MenuManager _menu;
    private readonly AdminService _admin;

    public GameEngine(
        GameContext context,
        MenuManager menu,
        AdminService admin)
    {
        _context = context;
        _menu = menu;
        _admin = admin;
    }

    // 🚫 NO override
    // 🚫 NO inheritance
    public void Start()
    {
        SeedIfEmpty();

        while (true)
        {
            _menu.ShowMainMenu(HandleMenuChoice);
        }
    }

    private void HandleMenuChoice(string choice)
    {
        switch (choice?.ToUpper())
        {
            case "1":
                _admin.AddCharacter();
                break;

            case "2":
                _admin.EditCharacter();
                break;

            case "3":
                _admin.DisplayAllCharacters();
                break;

            case "4":
                _admin.SearchCharacterByName();
                break;

            case "5":
                _admin.AddRoom();
                break;

            case "6":
                _admin.DisplayAllRooms();
                break;

            default:
                Console.WriteLine("Invalid option");
                break;
        }
    }

    private void SeedIfEmpty()
    {
        if (_context.Rooms.Any())
            return;

        var entrance = new Room { Name = "Entrance", Description = "Start room" };
        var hall = new Room { Name = "Hallway", Description = "Dark hall" };

        entrance.EastRoom = hall;
        hall.WestRoom = entrance;
        _context.Rooms.AddRange(entrance, hall);
        _context.SaveChanges();

        entrance.EastRoomId = hall.Id;
        hall.WestRoomId = entrance.Id;

        _context.SaveChanges();

    }
}

