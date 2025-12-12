using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Characters;

namespace ConsoleRpgEntities.Models.Abilities.PlayerAbilities
{
    public class ShoveAbility : Ability
    {
        public int Distance { get; set; }

        public ShoveAbility()
        {
            Name = "Shove";
            Attack = 10;
            AbilityType = "ShoveAbility";
            Distance = 2;
        }

        public override void Activate(Player user, ITargetable target)
        {
            target.Health -= Attack;
        }
    }
}
