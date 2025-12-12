using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Characters.Monsters;
using ConsoleRpgEntities.Models.Rooms;

public static class GameSeeder
{
    public static void Seed(GameContext context)
    {
        if (context.Rooms.Any())
            return; // ✅ DO NOT DELETE DB

        var entrance = new Room { Name = "Entrance", Description = "Start room" };
        var hall = new Room { Name = "Hallway", Description = "Dark hallway" };
        context.SlashAbilities.Add(new SlashAbility());
        context.SaveChanges();

        entrance.EastRoom = hall;
        hall.WestRoom = entrance;

        context.Rooms.AddRange(entrance, hall);
        context.SaveChanges();

        context.Monsters.Add(new Goblin
        {
            RoomId = hall.Id
        });

        context.SaveChanges();
    }
}
