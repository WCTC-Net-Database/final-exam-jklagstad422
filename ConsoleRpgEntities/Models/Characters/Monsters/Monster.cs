using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Rooms;

namespace ConsoleRpgEntities.Models.Characters.Monsters
{
    public abstract class Monster : IMonster, ITargetable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Health { get; set; }

        public int Attack { get; set; }      // ✅ PROPERTY
        public string MonsterType { get; set; }

        public int? RoomId { get; set; }
        public virtual Room Room { get; set; }

        public abstract void AttackTarget(ITargetable target); // ✅ METHOD
    }
}
