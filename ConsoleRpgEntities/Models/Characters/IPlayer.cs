using ConsoleRpgEntities.Models.Abilities;

namespace ConsoleRpgEntities.Models.Characters
{
    public interface IPlayer
    {
        int Id { get; set; }
        string Name { get; set; }
        int Health { get; set; }
        int Attack { get; set; }
        int Experience { get; set; }

        ICollection<Ability> Abilities { get; set; }
    }
}
