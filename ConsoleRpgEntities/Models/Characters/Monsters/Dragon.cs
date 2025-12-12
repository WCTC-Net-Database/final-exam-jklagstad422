using ConsoleRpgEntities.Models.Attributes;

namespace ConsoleRpgEntities.Models.Characters.Monsters
{
    public class Dragon : Monster
    {
        public Dragon()
        {
            Name = "Dragon";
            Health = 200;
            Attack = 25;
            MonsterType = "Dragon";
        }

        public override void AttackTarget(ITargetable target)
        {
            target.Health -= Attack;
        }
    }
}
