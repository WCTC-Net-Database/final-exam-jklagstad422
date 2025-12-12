using ConsoleRpgEntities.Models.Abilities;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Characters.Monsters;
using ConsoleRpgEntities.Models.Equipments;
using ConsoleRpgEntities.Models.Rooms;
using Microsoft.EntityFrameworkCore;

namespace ConsoleRpgEntities.Data
{
    public class GameContext : DbContext
    {
        public GameContext(DbContextOptions<GameContext> options)
            : base(options) { }

        // ---------------------------
        // CORE ENTITIES
        // ---------------------------
        public DbSet<Player> Players => Set<Player>();
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<Monster> Monsters => Set<Monster>();

        // ---------------------------
        // ABILITIES (CONCRETE ONLY)
        // ---------------------------
        public DbSet<SlashAbility> SlashAbilities => Set<SlashAbility>();
        // add more concrete abilities here if needed

        // ---------------------------
        // EQUIPMENT / ITEMS
        // ---------------------------
        public DbSet<Item> Items => Set<Item>();
        public DbSet<Equipment> Equipments => Set<Equipment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------------------------
            // MONSTER INHERITANCE (TPH)
            // ---------------------------
            modelBuilder.Entity<Monster>()
                .HasDiscriminator<string>("MonsterType")
                .HasValue<Goblin>("Goblin")
                .HasValue<Dragon>("Dragon");

            // ---------------------------
            // ABILITY INHERITANCE (TPH)
            // ---------------------------
            modelBuilder.Entity<Ability>()
                .HasDiscriminator<string>("AbilityType")
                .HasValue<SlashAbility>("Slash");

            // ---------------------------
            // PLAYER ↔ ABILITIES (M:M)
            // ---------------------------
            modelBuilder.Entity<Player>()
                .HasMany(p => p.Abilities)
                .WithMany(a => a.Players)
                .UsingEntity(j => j.ToTable("PlayerAbilities"));

            // ---------------------------
            // ROOM ↔ ROOM (SELF-REFERENCING)
            // ---------------------------
            modelBuilder.Entity<Room>()
                .HasOne(r => r.NorthRoom)
                .WithMany()
                .HasForeignKey(r => r.NorthRoomId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Room>()
                .HasOne(r => r.SouthRoom)
                .WithMany()
                .HasForeignKey(r => r.SouthRoomId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Room>()
                .HasOne(r => r.EastRoom)
                .WithMany()
                .HasForeignKey(r => r.EastRoomId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Room>()
                .HasOne(r => r.WestRoom)
                .WithMany()
                .HasForeignKey(r => r.WestRoomId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
