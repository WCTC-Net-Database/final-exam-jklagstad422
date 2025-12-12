using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Characters.Monsters;

namespace ConsoleRpgEntities.Models.Rooms;

public class Room
{
    public int Id { get; set; }

    public string Name { get; set; } = "";
    public string Description { get; set; } = "";

    // Direction FKs ONLY
    public int? NorthRoomId { get; set; }
    public int? SouthRoomId { get; set; }
    public int? EastRoomId { get; set; }
    public int? WestRoomId { get; set; }

    // Collections
    public virtual ICollection<Player> Players { get; set; } = new List<Player>();
    public virtual ICollection<Monster> Monsters { get; set; } = new List<Monster>();
}
