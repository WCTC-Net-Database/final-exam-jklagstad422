using ConsoleRpgEntities.Models.Abilities;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Characters.Monsters;
using ConsoleRpgEntities.Models.Rooms;
using Microsoft.EntityFrameworkCore;

namespace ConsoleRpgEntities.Data
{
    public class GameContext : DbContext
    {
        public GameContext(DbContextOptions<GameContext> options)
            : base(options) { }

        public DbSet<Player> Players => Set<Player>();
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<Monster> Monsters => Set<Monster>();

        // ✅ REGISTER CONCRETE ABILITY
        public DbSet<SlashAbility> SlashAbilities => Set<SlashAbility>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------------------------
            // MONSTERS (TPH)
            // ---------------------------
            modelBuilder.Entity<Monster>()
                .HasDiscriminator<string>("MonsterType")
                .HasValue<Goblin>("Goblin")
                .HasValue<Dragon>("Dragon");

            // ---------------------------
            // ABILITIES (TPH)
            // ---------------------------
            modelBuilder.Entity<Ability>()
                .HasDiscriminator<string>("AbilityType")
                .HasValue<SlashAbility>("Slash");

            // ---------------------------
            // PLAYER ↔ ABILITIES
            // ---------------------------
            modelBuilder.Entity<Player>()
                .HasMany(p => p.Abilities)
                .WithMany(a => a.Players)
                .UsingEntity(j => j.ToTable("PlayerAbilities"));

            // ---------------------------
            // ROOM SELF-REFERENCES
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
