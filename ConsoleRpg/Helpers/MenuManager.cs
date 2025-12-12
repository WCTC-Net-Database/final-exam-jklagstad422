using ConsoleRpg.Helpers;

namespace ConsoleRpg.Helpers
{
    public class MenuManager
    {
        private readonly OutputManager _outputManager;

        public MenuManager(OutputManager outputManager)
        {
            _outputManager = outputManager;
        }

        public void ShowMainMenu(Action<string> handleChoice)
        {
            _outputManager.Clear();

            _outputManager.WriteLine("=================================", ConsoleColor.Yellow);
            _outputManager.WriteLine("        ADMIN / DEV MENU", ConsoleColor.Yellow);
            _outputManager.WriteLine("=================================", ConsoleColor.Yellow);
            _outputManager.WriteLine("");

            _outputManager.WriteLine("E. Enter Exploration Mode", ConsoleColor.Green);
            _outputManager.WriteLine("");

            _outputManager.WriteLine("BASIC FEATURES:", ConsoleColor.Cyan);
            _outputManager.WriteLine("1. Add Character");
            _outputManager.WriteLine("2. Edit Character");
            _outputManager.WriteLine("3. Display All Characters");
            _outputManager.WriteLine("4. Search Character by Name");
            _outputManager.WriteLine("");

            _outputManager.WriteLine("C-LEVEL FEATURES:", ConsoleColor.Cyan);
            _outputManager.WriteLine("5. Add Ability to Character");
            _outputManager.WriteLine("6. Display Character Abilities");
            _outputManager.WriteLine("");

            _outputManager.WriteLine("B-LEVEL FEATURES:", ConsoleColor.Cyan);
            _outputManager.WriteLine("7. Add Room");
            _outputManager.WriteLine("8. Edit Room");
            _outputManager.WriteLine("9. Add Monster to Room");
            _outputManager.WriteLine("10. Display Room Details");
            _outputManager.WriteLine("");

            _outputManager.WriteLine("Select an option:");
            _outputManager.Display();

            var input = Console.ReadLine();
            handleChoice(input);
        }
    }
}
