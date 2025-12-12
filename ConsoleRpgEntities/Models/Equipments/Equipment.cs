
namespace ConsoleRpgEntities.Models.Equipments
{
    public class Equipment
    {
        public int Id { get; set; }

        public int? WeaponId { get; set; }
        public int? ArmorId { get; set; }

        public virtual Item Weapon { get; set; }
        public virtual Item Armor { get; set; }
    }
}
