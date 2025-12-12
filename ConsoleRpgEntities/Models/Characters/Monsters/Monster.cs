using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Rooms;

namespace ConsoleRpgEntities.Models.Characters.Monsters;

public abstract class Monster : ITargetable
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Health { get; set; }

    public int AttackPower { get; set; }   // 🔑 RENAMED

    public int? RoomId { get; set; }
    public Room? Room { get; set; }

    public abstract void Attack(ITargetable target);
}
