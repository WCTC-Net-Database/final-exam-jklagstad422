using ConsoleRpgEntities.Models.Rooms;
using Spectre.Console;

namespace ConsoleRpg.Helpers;

/// <summary>
/// Manages the exploration mode UI layout and rendering
/// Separated from GameEngine to follow Single Responsibility Principle
/// </summary>
public class ExplorationUI
{
    private readonly MapManager _mapManager;

    private readonly List<string> _messageLog = new();
    private readonly List<string> _outputLog = new();

    private const int MaxMessages = 8;
    private const int MaxOutputLines = 15;

    public ExplorationUI(MapManager mapManager)
    {
        _mapManager = mapManager;
    }

    /// <summary>
    /// Renders the exploration UI and returns the selected player action
    /// </summary>
    public string RenderAndGetAction(IEnumerable<Room> allRooms, Room currentRoom)
    {
        AnsiConsole.Clear();

        // ---------------------------
        // Map Panel
        // ---------------------------
        var mapPanel = _mapManager.GetCompactMapPanel(allRooms, currentRoom);
        AnsiConsole.Write(mapPanel);

        // ---------------------------
        // Room Details Panel
        // ---------------------------
        var roomPanel = _mapManager.GetCompactRoomDetailsPanel(currentRoom);
        AnsiConsole.Write(roomPanel);

        AnsiConsole.WriteLine();

        // ---------------------------
        // Output Log
        // ---------------------------
        if (_outputLog.Any())
        {
            foreach (var line in _outputLog)
            {
                AnsiConsole.MarkupLine(line);
            }
            _outputLog.Clear();
        }

        // ---------------------------
        // Available Actions
        // ---------------------------
        var actions = _mapManager.GetAvailableActions(currentRoom);

        for (int i = 0; i < actions.Count; i++)
        {
            AnsiConsole.MarkupLine($"[cyan]{i + 1}[/]. {actions[i]}");
        }

        int choice = AnsiConsole.Ask<int>(
            "[white]Enter the number of your action:[/]", 1);

        // Clamp choice to valid range
        if (choice < 1 || choice > actions.Count)
            choice = 1;

        return actions[choice - 1];
    }

    // =====================================================
    // MESSAGE / OUTPUT LOGGING
    // =====================================================

    /// <summary>
    /// Adds a concise message to the summary message log
    /// </summary>
    public void AddMessage(string message)
    {
        _messageLog.Add($"[dim]{DateTime.Now:HH:mm:ss}[/] {message}");

        if (_messageLog.Count > MaxMessages)
        {
            _messageLog.RemoveAt(0);
        }
    }

    /// <summary>
    /// Adds detailed output to the output log
    /// </summary>
    public void AddOutput(string output)
    {
        _outputLog.Add(output);

        if (_outputLog.Count > MaxOutputLines)
        {
            _outputLog.RemoveAt(0);
        }
    }

    /// <summary>
    /// Clears summary messages
    /// </summary>
    public void ClearMessages()
    {
        _messageLog.Clear();
    }

    /// <summary>
    /// Clears detailed output
    /// </summary>
    public void ClearOutput()
    {
        _outputLog.Clear();
    }
}
