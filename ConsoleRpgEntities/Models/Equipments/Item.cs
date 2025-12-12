namespace ConsoleRpgEntities.Models.Equipments
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Power { get; set; }

        // ❌ DO NOT add navigation back to Equipment
        // public ICollection<Equipment> Equipments { get; set; }
    }
}
