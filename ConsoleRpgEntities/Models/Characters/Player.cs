using ConsoleRpgEntities.Models.Abilities;
using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Equipments;
using ConsoleRpgEntities.Models.Rooms;

namespace ConsoleRpgEntities.Models.Characters;

public class Player : ITargetable, IPlayer
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    public int Health { get; set; }
    public int Attack { get; set; }
    public int Experience { get; set; }

    // Foreign Keys
    public int? EquipmentId { get; set; }
    public int? RoomId { get; set; }

    // 🔑 NAVIGATION PROPERTIES MUST BE VIRTUAL
    public virtual Equipment? Equipment { get; set; }
    public virtual ICollection<Ability> Abilities { get; set; } = new List<Ability>();
    public virtual Room? Room { get; set; }

    // Required for EF proxy creation
    public Player() { }
}
