namespace ConsoleRpgEntities.Models.Attributes;

public interface ITargetable
{
    string Name { get; }
    int Health { get; set; }
}
