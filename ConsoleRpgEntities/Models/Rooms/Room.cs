using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Characters.Monsters;

public class Room
{
    public int Id { get; set; }

    public string Name { get; set; } = "";
    public string Description { get; set; } = "";

    // 🔑 Foreign keys
    public int? NorthRoomId { get; set; }
    public int? SouthRoomId { get; set; }
    public int? EastRoomId { get; set; }
    public int? WestRoomId { get; set; }

    // 🔑 Navigation properties
    public Room? NorthRoom { get; set; }
    public Room? SouthRoom { get; set; }
    public Room? EastRoom { get; set; }
    public Room? WestRoom { get; set; }

    public ICollection<Player> Players { get; set; } = new List<Player>();
    public ICollection<Monster> Monsters { get; set; } = new List<Monster>();
}
