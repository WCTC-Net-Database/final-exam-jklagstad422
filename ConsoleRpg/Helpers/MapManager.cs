using ConsoleRpgEntities.Models.Rooms;
using Spectre.Console;

namespace ConsoleRpg.Helpers
{
    public class MapManager
    {
        public Panel GetCompactMapPanel(IEnumerable<Room> rooms, Room currentRoom)
        {
            var text = $"[bold yellow]Current Room:[/] [green]{currentRoom.Name}[/]\n";

            foreach (var room in rooms)
            {
                text += room.Id == currentRoom.Id
                    ? $"[green]> {room.Name}[/]\n"
                    : $"  {room.Name}\n";
            }

            return new Panel(text)
            {
                Border = BoxBorder.Rounded
            };
        }

        public Panel GetCompactRoomDetailsPanel(Room room)
        {
            var text = $"[cyan]{room.Description}[/]\n\n";

            if (room.Monsters.Any())
            {
                text += "[red]Monsters:[/]\n";
                foreach (var m in room.Monsters)
                    text += $" • {m.Name} (HP: {m.Health})\n";
            }
            else
            {
                text += "[green]No monsters here.[/]\n";
            }

            return new Panel(text)
            {
                Header = new PanelHeader("[green]Room Details[/]"),
                Border = BoxBorder.Rounded
            };
        }

        public List<string> GetAvailableActions(Room room)
        {
            var actions = new List<string>();

            if (room.NorthRoomId != null) actions.Add("Go North");
            if (room.SouthRoomId != null) actions.Add("Go South");
            if (room.EastRoomId != null) actions.Add("Go East");
            if (room.WestRoomId != null) actions.Add("Go West");

            actions.Add("View Inventory");
            actions.Add("View Character Stats");

            if (room.Monsters.Any())
            {
                actions.Add("Attack Monster");
                actions.Add("Use Ability");
            }

            actions.Add("Return to Admin Mode");

            return actions;
        }
    }
}
