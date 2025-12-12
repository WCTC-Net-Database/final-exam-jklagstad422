using ConsoleRpg.Helpers;
using ConsoleRpg.Models;
using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace ConsoleRpg.Services;

public class GameEngine
{
    private readonly GameContext _context;
    private readonly MenuManager _menuManager;
    private readonly MapManager _mapManager;
    private readonly ExplorationUI _explorationUI;
    private readonly PlayerService _playerService;
    private readonly AdminService _adminService;
    private readonly ILogger<GameEngine> _logger;

    private Player _currentPlayer;
    private Room _currentRoom;
    private GameMode _currentMode = GameMode.Exploration;

    public GameEngine(
        GameContext context,
        MenuManager menuManager,
        MapManager mapManager,
        ExplorationUI explorationUI,
        PlayerService playerService,
        AdminService adminService,
        ILogger<GameEngine> logger)
    {
        _context = context;
        _menuManager = menuManager;
        _mapManager = mapManager;
        _explorationUI = explorationUI;
        _playerService = playerService;
        _adminService = adminService;
        _logger = logger;
    }

    // =====================================================
    // APPLICATION ENTRY POINT
    // =====================================================
    // FIX: Start() now correctly boots the game instead of crashing
    internal void Start()
    {
        Run();
    }

    // =====================================================
    // MAIN GAME LOOP
    // =====================================================
    public void Run()
    {
        _logger.LogInformation("Game engine started");

        InitializeGame();

        while (true)
        {
            if (_currentMode == GameMode.Exploration)
            {
                ExplorationMode();
            }
            else
            {
                AdminMode();
            }
        }
    }

    // =====================================================
    // INITIALIZATION
    // =====================================================
    private void InitializeGame()
    {
        _currentPlayer = _context.Players
            .Include(p => p.Room)
            .Include(p => p.Equipment)
            .Include(p => p.Abilities)
            .FirstOrDefault();

        if (_currentPlayer == null)
        {
            AnsiConsole.MarkupLine("[yellow]No players found! Please create a character first.[/]");
            _currentMode = GameMode.Admin;
            return;
        }

        _currentRoom = _currentPlayer.Room
            ?? _context.Rooms
                .Include(r => r.Players)
                .Include(r => r.Monsters)
                .FirstOrDefault();

        if (_currentRoom == null)
        {
            AnsiConsole.MarkupLine("[red]No rooms found! Database may not be properly seeded.[/]");
            _currentMode = GameMode.Admin;
            return;
        }

        _logger.LogInformation(
            "Game initialized with player {PlayerName} in room {RoomName}",
            _currentPlayer.Name,
            _currentRoom.Name);
    }

    // =====================================================
    // EXPLORATION MODE
    // =====================================================
    private void ExplorationMode()
    {
        _currentRoom = _context.Rooms
            .Include(r => r.Players)
            .Include(r => r.Monsters)
            .Include(r => r.NorthRoom)
            .Include(r => r.SouthRoom)
            .Include(r => r.EastRoom)
            .Include(r => r.WestRoom)
            .FirstOrDefault(r => r.Id == _currentRoom.Id);

        var allRooms = _context.Rooms.ToList();
        bool hasMonsters = _currentRoom.Monsters != null && _currentRoom.Monsters.Any();

        var selectedAction = _explorationUI.RenderAndGetAction(allRooms, _currentRoom);
        HandleExplorationAction(selectedAction, hasMonsters);
    }

    private void HandleExplorationAction(string action, bool hasMonsters)
    {
        switch (action)
        {
            case "Go North":
                HandleMoveResult(_playerService.MoveToRoom(
                    _currentPlayer, _currentRoom, _currentRoom.NorthRoomId, "North"));
                break;

            case "Go South":
                HandleMoveResult(_playerService.MoveToRoom(
                    _currentPlayer, _currentRoom, _currentRoom.SouthRoomId, "South"));
                break;

            case "Go East":
                HandleMoveResult(_playerService.MoveToRoom(
                    _currentPlayer, _currentRoom, _currentRoom.EastRoomId, "East"));
                break;

            case "Go West":
                HandleMoveResult(_playerService.MoveToRoom(
                    _currentPlayer, _currentRoom, _currentRoom.WestRoomId, "West"));
                break;

            case "View Map":
                _explorationUI.AddMessage("[cyan]Viewing map[/]");
                _explorationUI.AddOutput("[cyan]The map is displayed above.[/]");
                break;

            case "View Inventory":
                HandleActionResult(_playerService.ShowInventory(_currentPlayer));
                break;

            case "View Character Stats":
                HandleActionResult(_playerService.ShowCharacterStats(_currentPlayer));
                break;

            case "Attack Monster":
                HandleActionResult(_playerService.AttackMonster());
                break;

            case "Use Ability":
                HandleActionResult(_playerService.UseAbilityOnMonster());
                break;

            case "Return to Main Menu":
                _currentMode = GameMode.Admin;
                _explorationUI.AddMessage("[yellow]→ Admin Mode[/]");
                _explorationUI.AddOutput("[yellow]Switching to Admin Mode.[/]");
                break;

            default:
                _explorationUI.AddMessage("[red]Unknown action[/]");
                _explorationUI.AddOutput($"[red]Unknown action: {action}[/]");
                break;
        }
    }

    private void HandleMoveResult(ServiceResult<Room> result)
    {
        _explorationUI.AddMessage(result.Message);
        _explorationUI.AddOutput(result.DetailedOutput);

        if (result.Success && result.Value != null)
        {
            _currentRoom = result.Value;
        }
    }

    private void HandleActionResult(ServiceResult result)
    {
        _explorationUI.AddMessage(result.Message);
        _explorationUI.AddOutput(result.DetailedOutput);
    }

    // =====================================================
    // ADMIN MODE
    // =====================================================
    private void AdminMode()
    {
        _menuManager.ShowMainMenu(AdminMenuChoice);
    }

    private void AdminMenuChoice(string choice)
    {
        switch (choice?.ToUpper())
        {
            case "E":
            case "0":
                ExploreWorld();
                break;

            case "1":
                _adminService.AddCharacter();
                break;

            case "2":
                _adminService.EditCharacter();
                break;

            case "3":
                _adminService.DisplayAllCharacters();
                PressAnyKey();
                break;

            case "4":
                _adminService.SearchCharacterByName();
                PressAnyKey();
                break;

            case "5":
                _adminService.AddAbilityToCharacter();
                break;

            case "6":
                _adminService.DisplayCharacterAbilities();
                break;

            case "7":
                AnsiConsole.MarkupLine("[yellow]Use this in Exploration Mode[/]");
                PressAnyKey();
                break;

            case "8":
                _adminService.AddRoom();
                break;

            case "9":
                _adminService.DisplayRoomDetails();
                PressAnyKey();
                break;

            default:
                AnsiConsole.MarkupLine("[red]Invalid selection.[/]");
                PressAnyKey();
                break;
        }
    }

    // =====================================================
    // MODE SWITCHING
    // =====================================================
    private void ExploreWorld()
    {
        _logger.LogInformation("Switching to Exploration Mode");

        _currentMode = GameMode.Exploration;
        _explorationUI.AddMessage("[green]Entered world[/]");
        _explorationUI.AddOutput("[green]Welcome to the world![/]");
    }

    // =====================================================
    // HELPERS
    // =====================================================
    private void PressAnyKey()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Markup("[dim]Press any key to continue...[/]");
        Console.ReadKey(true);
    }
}

// =========================================================
// GAME MODE ENUM
// =========================================================
public enum GameMode
{
    Exploration,
    Admin
}
