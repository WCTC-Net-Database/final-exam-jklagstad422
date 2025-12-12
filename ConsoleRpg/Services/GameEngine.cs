using ConsoleRpg.Helpers;
using ConsoleRpg.Services;
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

    // =====================================================
    // ENTRY POINT
    // =====================================================
    public void Start()
    {
        SeedIfEmpty();   // 🔑 SEED ONCE, SAFELY

        while (true)
        {
            _menu.ShowMainMenu(HandleMenuChoice);
        }
    }

    // =====================================================
    // ADMIN MENU HANDLER
    // =====================================================
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
                Console.WriteLine("Invalid option.");
                Console.ReadKey(true);
                break;
        }
    }

    // =====================================================
    // SAFE ROOM SEEDING (NO CIRCULAR DEPENDENCY)
    // =====================================================
    private void SeedIfEmpty()
    {
        if (_context.Rooms.Any())
            return;

        // STEP 1 — create rooms WITHOUT links
        var entrance = new Room
        {
            Name = "Entrance",
            Description = "The starting room"
        };

        var hallway = new Room
        {
            Name = "Hallway",
            Description = "A dark hallway"
        };

        _context.Rooms.AddRange(entrance, hallway);
        _context.SaveChanges();   // ✅ IDs generated here

        // STEP 2 — now link using IDs
        entrance.EastRoomId = hallway.Id;
        hallway.WestRoomId = entrance.Id;

        _context.SaveChanges();   // ✅ SAFE
    }

}
