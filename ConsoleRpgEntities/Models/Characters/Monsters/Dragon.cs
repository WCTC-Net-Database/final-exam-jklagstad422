using System.Xml.Linq;
using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Characters.Monsters;

public class Dragon : Monster
{
    public Dragon()
    {
        Name = "Dragon";
        Health = 200;
        AttackPower = 25;
    }

    public override void Attack(ITargetable target)
    {
        target.Health -= AttackPower;
    }
}
