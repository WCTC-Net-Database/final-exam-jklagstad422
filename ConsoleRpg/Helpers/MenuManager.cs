namespace ConsoleRpg.Helpers;

public class MenuManager
{
    public void ShowMainMenu(Action<string> handler)
    {
        Console.Clear();
        Console.WriteLine("ADMIN MENU");
        Console.WriteLine("1. Add Character");
        Console.WriteLine("2. Edit Character");
        Console.WriteLine("3. Display Characters");
        Console.WriteLine("4. Search Character");
        Console.WriteLine("5. Add Room");
        Console.WriteLine("6. Display Rooms");
        Console.WriteLine("E. Enter Game");

        handler(Console.ReadLine() ?? "");
    }
}
