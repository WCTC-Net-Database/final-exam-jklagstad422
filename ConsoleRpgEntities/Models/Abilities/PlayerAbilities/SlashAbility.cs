using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Characters;

namespace ConsoleRpgEntities.Models.Abilities;

public class SlashAbility : Ability
{
    public SlashAbility()
    {
        Name = "Slash";
        Attack = 10;
        AbilityType = "Slash"; // 🔑 REQUIRED for EF discriminator
    }

    public override void Activate(Player user, ITargetable target)
    {
        if (target == null)
            return;

        int damage = Math.Max(Attack, 1);

        target.Health -= damage;

        Console.WriteLine(
            $"{user.Name} uses {Name} and deals {damage} damage to {target.Name}!");

        if (target.Health <= 0)
        {
            Console.WriteLine($"{target.Name} has been defeated!");
        }
    }
}
