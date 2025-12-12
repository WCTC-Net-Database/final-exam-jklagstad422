using System;
using System.Linq;
using ConsoleRpg.Models;
using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Characters.Monsters;
using ConsoleRpgEntities.Models.Abilities;
using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ConsoleRpg.Services
{
    /// <summary>
    /// Handles all player-related actions and interactions.
    /// Returns ServiceResult objects to decouple logic from UI.
    /// </summary>
    public class PlayerService
    {
        private readonly GameContext _context;
        private readonly ILogger<PlayerService> _logger;

        public PlayerService(GameContext context, ILogger<PlayerService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // =====================================================
        // ROOM NAVIGATION
        // =====================================================
        public ServiceResult<Room> MoveToRoom(
            Player player,
            Room currentRoom,
            int? roomId,
            string direction)
        {
            try
            {
                if (player == null || currentRoom == null)
                {
                    return ServiceResult<Room>.Fail(
                        "[red]Movement failed[/]",
                        "[red]Invalid player or room state.[/]");
                }

                if (!roomId.HasValue)
                {
                    return ServiceResult<Room>.Fail(
                        $"[red]Cannot go {direction}[/]",
                        $"[red]You cannot go {direction} from here.[/]");
                }

                var newRoom = _context.Rooms
                    .Include(r => r.Players)
                    .Include(r => r.Monsters)
                    .Include(r => r.NorthRoom)
                    .Include(r => r.SouthRoom)
                    .Include(r => r.EastRoom)
                    .Include(r => r.WestRoom)
                    .FirstOrDefault(r => r.Id == roomId.Value);

                if (newRoom == null)
                {
                    return ServiceResult<Room>.Fail(
                        "[red]Room not found[/]",
                        "[red]That room does not exist.[/]");
                }

                var trackedPlayer = _context.Players.First(p => p.Id == player.Id);
                trackedPlayer.RoomId = newRoom.Id;

                _context.SaveChanges();

                _logger.LogInformation(
                    "Player {Player} moved {Direction} to {Room}",
                    trackedPlayer.Name, direction, newRoom.Name);

                return ServiceResult<Room>.Ok(
                    newRoom,
                    $"[green]→ {direction}[/]",
                    $"[green]You travel {direction} and arrive at {newRoom.Name}.[/]");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MoveToRoom failed");

                return ServiceResult<Room>.Fail(
                    "[red]Movement failed[/]",
                    $"[red]{ex.Message}[/]");
            }
        }

        // =====================================================
        // CHARACTER INFO
        // =====================================================
        public ServiceResult ShowCharacterStats(Player player)
        {
            var output =
                $"[yellow]Character:[/] {player.Name}\n" +
                $"[green]Health:[/] {player.Health}\n" +
                $"[cyan]Base Attack:[/] {player.Attack}\n" +
                $"[blue]Experience:[/] {player.Experience}";

            return ServiceResult.Ok(
                "[cyan]Viewing stats[/]",
                output);
        }

        // =====================================================
        // INVENTORY
        // =====================================================
        public ServiceResult ShowInventory(Player player)
        {
            var output =
                $"[magenta]Equipment:[/] {(player.Equipment != null ? "Equipped" : "None")}\n" +
                $"[blue]Abilities:[/] {player.Abilities?.Count ?? 0}";

            return ServiceResult.Ok(
                "[magenta]Viewing inventory[/]",
                output);
        }

        // =====================================================
        // COMBAT – BASE ATTACK
        // =====================================================
        public ServiceResult AttackMonster()
        {
            try
            {
                var player = _context.Players.FirstOrDefault();
                if (player == null || player.RoomId == null)
                {
                    return ServiceResult.Fail(
                        "[red]Attack failed[/]",
                        "[red]Player or room not found.[/]");
                }

                var monster = _context.Monsters
                    .FirstOrDefault(m => m.RoomId == player.RoomId);

                if (monster == null)
                {
                    return ServiceResult.Fail(
                        "[yellow]No monsters[/]",
                        "[yellow]There are no monsters here.[/]");
                }

                int damage = Math.Max(player.Attack, 1);
                monster.Health -= damage;

                string output =
                    $"[green]You attack the {monster.Name} for {damage} damage![/]\n";

                if (monster.Health <= 0)
                {
                    output += $"[bold red]{monster.Name} has been defeated![/]\n";
                    _context.Monsters.Remove(monster);
                }
                else
                {
                    output +=
                        $"[yellow]{monster.Name} HP remaining: {monster.Health}[/]\n";
                }

                _context.SaveChanges();

                _logger.LogInformation(
                    "Player attacked {Monster} for {Damage}",
                    monster.Name, damage);

                return ServiceResult.Ok(
                    "[red]Attack![/]",
                    output);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AttackMonster failed");

                return ServiceResult.Fail(
                    "[red]Attack failed[/]",
                    $"[red]{ex.Message}[/]");
            }
        }

        // =====================================================
        // COMBAT – ABILITY ATTACK (ABILITY HAS OWN ATTACK)
        // =====================================================
        public ServiceResult UseAbilityOnMonster()
        {
            try
            {
                var player = _context.Players
                    .Include(p => p.Abilities)
                    .FirstOrDefault();

                if (player == null || player.RoomId == null)
                {
                    return ServiceResult.Fail(
                        "[red]Ability failed[/]",
                        "[red]Player or room not found.[/]");
                }

                if (player.Abilities == null || !player.Abilities.Any())
                {
                    return ServiceResult.Fail(
                        "[yellow]No abilities[/]",
                        "[yellow]You do not have any abilities to use.[/]");
                }

                var monster = _context.Monsters
                    .FirstOrDefault(m => m.RoomId == player.RoomId);

                if (monster == null)
                {
                    return ServiceResult.Fail(
                        "[yellow]No monster[/]",
                        "[yellow]There is no monster to target.[/]");
                }

                // Simple selection: first ability
                var ability = player.Abilities.First();

                int damage = Math.Max(ability.Attack, 1);
                monster.Health -= damage;

                string output =
                    $"[cyan]You use {ability.Name}![/]\n" +
                    $"[green]It deals {damage} damage to {monster.Name}![/]\n";

                if (monster.Health <= 0)
                {
                    output += $"[bold red]{monster.Name} has been defeated![/]\n";
                    _context.Monsters.Remove(monster);
                }
                else
                {
                    output +=
                        $"[yellow]{monster.Name} HP remaining: {monster.Health}[/]\n";
                }

                _context.SaveChanges();

                _logger.LogInformation(
                    "Player used ability {Ability} on {Monster} for {Damage}",
                    ability.Name, monster.Name, damage);

                return ServiceResult.Ok(
                    "[blue]Ability used![/]",
                    output);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UseAbilityOnMonster failed");

                return ServiceResult.Fail(
                    "[red]Ability failed[/]",
                    $"[red]{ex.Message}[/]");
            }
        }
    }
}
