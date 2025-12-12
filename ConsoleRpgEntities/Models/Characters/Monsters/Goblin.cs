using System.Xml.Linq;
using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Characters.Monsters;

public class Goblin : Monster
{
    public Goblin()
    {
        Name = "Goblin";
        Health = 25;
        AttackPower = 5;
    }

    public override void Attack(ITargetable target)
    {
        target.Health -= AttackPower;
    }
}
