using ConsoleRpgEntities.Models.Abilities;
using ConsoleRpgEntities.Models.Abilities.PlayerAbilities;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Characters.Monsters;
using ConsoleRpgEntities.Models.Equipments;
using ConsoleRpgEntities.Models.Rooms;
using Microsoft.EntityFrameworkCore;

namespace ConsoleRpgEntities.Data
{
    public class GameContext : DbContext
    {
        // ---------------------------
        // DbSets
        // ---------------------------
        public DbSet<Player> Players { get; set; }
        public DbSet<Monster> Monsters { get; set; }
        public DbSet<Ability> Abilities { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Room> Rooms { get; set; }

        public GameContext(DbContextOptions<GameContext> options)
            : base(options)
        {
        }

        // ---------------------------
        // Model Configuration
        // ---------------------------
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // =====================================================
            // MONSTER INHERITANCE (TPH)
            // =====================================================
            modelBuilder.Entity<Monster>()
                .HasDiscriminator<string>("MonsterType")
                .HasValue<Goblin>("Goblin")
                .HasValue<Dragon>("Dragon");

            // =====================================================
            // ABILITY INHERITANCE (TPH)
            // =====================================================
            modelBuilder.Entity<Ability>()
                .HasDiscriminator<string>("AbilityType")
                .HasValue<ShoveAbility>("ShoveAbility");

            // =====================================================
            // PLAYER ↔ ABILITY (MANY-TO-MANY)
            // =====================================================
            modelBuilder.Entity<Player>()
                .HasMany(p => p.Abilities)
                .WithMany(a => a.Players)
                .UsingEntity(j => j.ToTable("PlayerAbilities"));

            // =====================================================
            // ROOM ↔ PLAYER
            // =====================================================
            modelBuilder.Entity<Room>()
                .HasMany(r => r.Players)
                .WithOne(p => p.Room)
                .HasForeignKey(p => p.RoomId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // =====================================================
            // ROOM ↔ MONSTER
            // =====================================================
            modelBuilder.Entity<Room>()
                .HasMany(r => r.Monsters)
                .WithOne(m => m.Room)
                .HasForeignKey(m => m.RoomId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // =====================================================
            // 🔑 SELF-REFERENCING ROOM EXITS (REQUIRED)
            // =====================================================
            modelBuilder.Entity<Room>()
                .HasOne(r => r.NorthRoom)
                .WithMany()
                .HasForeignKey(r => r.NorthRoomId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<Room>()
                .HasOne(r => r.SouthRoom)
                .WithMany()
                .HasForeignKey(r => r.SouthRoomId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<Room>()
                .HasOne(r => r.EastRoom)
                .WithMany()
                .HasForeignKey(r => r.EastRoomId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<Room>()
                .HasOne(r => r.WestRoom)
                .WithMany()
                .HasForeignKey(r => r.WestRoomId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
