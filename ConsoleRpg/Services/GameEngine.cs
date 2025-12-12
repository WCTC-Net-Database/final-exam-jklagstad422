using ConsoleRpg.Helpers;
using ConsoleRpg.Models;
using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace ConsoleRpg.Services
{
    public class GameEngine
    {
        private readonly GameContext _context;
        private readonly MenuManager _menuManager;
        private readonly ExplorationUI _explorationUI;
        private readonly PlayerService _playerService;
        private readonly AdminService _adminService;
        private readonly ILogger<GameEngine> _logger;

        private Player? _currentPlayer;
        private Room? _currentRoom;
        private GameMode _mode = GameMode.Admin;

        public GameEngine(
            GameContext context,
            MenuManager menuManager,
            ExplorationUI explorationUI,
            PlayerService playerService,
            AdminService adminService,
            ILogger<GameEngine> logger)
        {
            _context = context;
            _menuManager = menuManager;
            _explorationUI = explorationUI;
            _playerService = playerService;
            _adminService = adminService;
            _logger = logger;
        }

        public void Start()
        {
            InitializeGame();

            while (true)
            {
                if (_mode == GameMode.Exploration)
                    RunExploration();
                else
                    RunAdmin();
            }
        }

        private void InitializeGame()
        {
            _currentPlayer = _context.Players
                .Include(p => p.Abilities)
                .FirstOrDefault();

            _currentRoom = _context.Rooms
                .Include(r => r.Players)
                .Include(r => r.Monsters)
                .FirstOrDefault();

            if (_currentPlayer == null || _currentRoom == null)
                _mode = GameMode.Admin;
        }

        // =====================================================
        // EXPLORATION MODE
        // =====================================================
        private void RunExploration()
        {
            if (_currentRoom == null || _currentPlayer == null)
            {
                _mode = GameMode.Admin;
                return;
            }

            _currentRoom = _context.Rooms
                .Include(r => r.Players)
                .Include(r => r.Monsters)
                .Include(r => r.NorthRoom)
                .Include(r => r.SouthRoom)
                .Include(r => r.EastRoom)
                .Include(r => r.WestRoom)
                .First(r => r.Id == _currentRoom.Id);

            var allRooms = _context.Rooms.ToList();
            var action = _explorationUI.RenderAndGetAction(allRooms, _currentRoom);

            switch (action)
            {
                case "Go North":
                    Move(_currentRoom.NorthRoomId, "North");
                    break;
                case "Go South":
                    Move(_currentRoom.SouthRoomId, "South");
                    break;
                case "Go East":
                    Move(_currentRoom.EastRoomId, "East");
                    break;
                case "Go West":
                    Move(_currentRoom.WestRoomId, "West");
                    break;
                case "Attack Monster":
                    Show(_playerService.AttackMonster());
                    break;
                case "Use Ability":
                    Show(_playerService.UseAbilityOnMonster());
                    break;
                case "Return to Admin Mode":
                    _mode = GameMode.Admin;
                    break;
            }
        }

        private void Move(int? roomId, string direction)
        {
            var result = _playerService.MoveToRoom(
                _currentPlayer!, _currentRoom!, roomId, direction);

            _explorationUI.AddMessage(result.Message);
            _explorationUI.AddOutput(result.DetailedOutput);

            if (result.Success && result.Value != null)
                _currentRoom = result.Value;
        }

        private void Show(ServiceResult result)
        {
            _explorationUI.AddMessage(result.Message);
            _explorationUI.AddOutput(result.DetailedOutput);
        }

        // =====================================================
        // ADMIN MODE
        // =====================================================
        private void RunAdmin()
        {
            _menuManager.ShowMainMenu(choice =>
            {
                switch (choice)
                {
                    case "E":
                        InitializeGame();
                        _mode = GameMode.Exploration;
                        break;
                    case "1":
                        _adminService.AddCharacter();
                        break;
                    case "8":
                        _adminService.AddRoom();
                        break;
                }
            });
        }
    }

    public enum GameMode
    {
        Exploration,
        Admin
    }
}
