using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Characters.Monsters;
using ConsoleRpgEntities.Models.Rooms;

public static class GameSeeder
{
    public static void Seed(GameContext context)
    {
        // ✅ Do NOT reseed if data exists
        if (context.Rooms.Any())
            return;

        // -----------------------------
        // Create rooms (NO navigation properties)
        // -----------------------------
        var entrance = new Room
        {
            Name = "Entrance",
            Description = "Start room"
        };

        var hall = new Room
        {
            Name = "Hallway",
            Description = "Dark hallway"
        };

        context.Rooms.AddRange(entrance, hall);
        context.SaveChanges(); // IDs generated here

        // -----------------------------
        // Link rooms USING FK IDs ONLY
        // -----------------------------
        entrance.EastRoomId = hall.Id;
        hall.WestRoomId = entrance.Id;

        context.SaveChanges();

        // -----------------------------
        // Add monster to hallway
        // -----------------------------
        context.Monsters.Add(new Goblin
        {
            RoomId = hall.Id
        });

        context.SaveChanges();
    }
}
