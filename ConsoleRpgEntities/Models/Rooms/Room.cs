using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Characters.Monsters;

namespace ConsoleRpgEntities.Models.Rooms
{
    public class Room
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        // OPTIONAL coordinates (fixes DbUpdateException)
        public int? X { get; set; }
        public int? Y { get; set; }

        // Self-referencing exit foreign keys
        public int? NorthRoomId { get; set; }
        public int? SouthRoomId { get; set; }
        public int? EastRoomId { get; set; }
        public int? WestRoomId { get; set; }

        // Navigation properties
        public virtual Room? NorthRoom { get; set; }
        public virtual Room? SouthRoom { get; set; }
        public virtual Room? EastRoom { get; set; }
        public virtual Room? WestRoom { get; set; }

        // Inhabitants
        public virtual ICollection<Player> Players { get; set; } = new List<Player>();
        public virtual ICollection<Monster> Monsters { get; set; } = new List<Monster>();
    }
}
