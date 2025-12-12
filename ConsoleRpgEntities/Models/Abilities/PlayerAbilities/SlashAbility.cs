using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Characters;

public class SlashAbility : Ability
{
    public SlashAbility()
    {
        Name = "Slash";
        Attack = 10;
        AbilityType = "Slash";
    }

    public override void Activate(Player user, ITargetable target)
    {
        target.Health -= Attack;
    }
}
