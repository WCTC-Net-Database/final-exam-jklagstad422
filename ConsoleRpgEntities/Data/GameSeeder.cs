using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Characters.Monsters;
using ConsoleRpgEntities.Models.Rooms;

namespace ConsoleRpgEntities.Data
{
    public static class GameSeeder
    {
        public static void Seed(GameContext context)
        {
            // 🔥 DEV ONLY — guaranteed clean slate
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // ---------------------------
            // ROOMS (NO LINKS YET)
            // ---------------------------
            var entrance = new Room
            {
                Name = "Entrance",
                Description = "The entrance to the dungeon."
            };

            var hallway = new Room
            {
                Name = "Hallway",
                Description = "A long, dark hallway."
            };

            var treasure = new Room
            {
                Name = "Treasure Room",
                Description = "A room glittering with gold."
            };

            context.Rooms.AddRange(entrance, hallway, treasure);
            context.SaveChanges(); // 🔑 IDs generated here

            // ---------------------------
            // CONNECT ROOMS
            // ---------------------------
            entrance.EastRoomId = hallway.Id;
            hallway.WestRoomId = entrance.Id;

            hallway.EastRoomId = treasure.Id;
            treasure.WestRoomId = hallway.Id;

            context.SaveChanges();

            // ---------------------------
            // MONSTERS
            // ---------------------------
            var goblin = new Goblin
            {
                Health = 25,
                Attack = 5,
                RoomId = hallway.Id
            };

            var dragon = new Dragon
            {
                Health = 200,
                Attack = 25,
                RoomId = treasure.Id
            };

            context.Monsters.AddRange(goblin, dragon);

            // ---------------------------
            // PLAYER
            // ---------------------------
            var player = new Player
            {
                Name = "Hero",
                Health = 100,
                Attack = 10,
                Experience = 0,
                RoomId = entrance.Id
            };

            context.Players.Add(player);

            context.SaveChanges();
        }
    }
}
