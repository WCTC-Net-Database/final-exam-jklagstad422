using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Characters;

namespace ConsoleRpgEntities.Models.Abilities
{
    public interface IAbility
    {
        int Id { get; set; }
        string Name { get; set; }
        int Attack { get; set; }

        void Activate(Player user, ITargetable target);
    }
}
