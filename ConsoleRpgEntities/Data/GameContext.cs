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

        // -------------------------
        // DbSets (ONLY what you use)
        // -------------------------
        public DbSet<Player> Players => Set<Player>();
        public DbSet<Monster> Monsters => Set<Monster>();
        public DbSet<Room> Rooms => Set<Room>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // -------------------------
            // Monster inheritance (TPH)
            // -------------------------
            modelBuilder.Entity<Monster>()
                .HasDiscriminator<string>("MonsterType")
                .HasValue<Goblin>("Goblin")
                .HasValue<Dragon>("Dragon");

            // -------------------------
            // Room → Players
            // -------------------------
            modelBuilder.Entity<Room>()
                .HasMany(r => r.Players)
                .WithOne(p => p.Room)
                .HasForeignKey(p => p.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------------------
            // Room → Monsters
            // -------------------------
            modelBuilder.Entity<Room>()
                .HasMany(r => r.Monsters)
                .WithOne(m => m.Room)
                .HasForeignKey(m => m.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
