using ConsoleRpgEntities.Models.Abilities;
using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Characters;

public abstract class Ability : IAbility
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Attack { get; set; }
    public string AbilityType { get; set; }

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();

    public abstract void Activate(Player user, ITargetable target);
}
