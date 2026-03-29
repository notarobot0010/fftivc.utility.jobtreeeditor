using fftivc.utility.jobtreeeditor.shared.Layout;
using fftivc.utility.jobtreeeditor.uib;

namespace fftivc.utility.jobtreeeditor.cli;

/// <summary>
/// Interactive console interface for editing job tree positions.
/// </summary>
public class ConsoleEditor(JobTreeUib uib, string outputPath)
{
    private readonly JobTreeUib _uib = uib;
    private bool _hasUnsavedChanges;

    public void Run()
    {
        Console.Clear();
        PrintHeader();
        Console.WriteLine($"  Loaded: {_uib.SourcePath}");
        Console.WriteLine($"  Output: {outputPath}");
        Console.WriteLine();

        MainLoop();
    }

    private void MainLoop()
    {
        while (true)
        {
            PrintJobTable();
            Console.WriteLine();
            PrintMainMenu();

            string? input = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(input)) continue;

            switch (input.ToLower())
            {
                case "q":
                    if (ConfirmExit()) return;
                    break;
                case "s":
                    Save();
                    break;
                case "r":
                    ResetAll();
                    break;
                case "e":
                    ExportLayout();
                    break;
                case "l":
                    LoadLayout();
                    break;
                default:
                    if (int.TryParse(input, out int jobNum) && jobNum >= 1 && jobNum <= 20)
                        EditJob(UibConstants.Jobs[jobNum - 1]);
                    else
                        PrintError("Invalid input. Enter a job number (1-20) or a menu command.");
                    break;
            }
        }
    }

    private void EditJob(JobSlot slot)
    {
        var pos = _uib.ReadPosition(slot);

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"  === Editing: {slot.Name} ===");
        Console.ResetColor();
        Console.WriteLine($"  Current position: {pos}");
        Console.WriteLine($"  Default position: ({slot.DefaultX}, {slot.DefaultY})");
        Console.WriteLine();
        Console.WriteLine("  [1] Swap with another job");
        Console.WriteLine("  [2] Set manual X, Y");
        Console.WriteLine("  [3] Reset to default");
        Console.WriteLine("  [0] Cancel");
        Console.Write("  > ");

        string? choice = Console.ReadLine()?.Trim();
        switch (choice)
        {
            case "1":
                SwapWithJob(slot);
                break;
            case "2":
                ManualPosition(slot);
                break;
            case "3":
                _uib.ResetPositionToDefault(slot);
                _hasUnsavedChanges = true;
                PrintSuccess($"{slot.Name} reset to default ({slot.DefaultX}, {slot.DefaultY}).");
                break;
            case "0":
                break;
            default:
                PrintError("Invalid choice.");
                break;
        }
    }

    private void SwapWithJob(JobSlot sourceSlot)
    {
        Console.WriteLine();
        Console.WriteLine("  Select job to swap with:");
        for (int i = 0; i < UibConstants.Jobs.Length; i++)
        {
            var s = UibConstants.Jobs[i];
            if (s == sourceSlot) continue;
            var p = _uib.ReadPosition(s);
            Console.WriteLine($"    [{i + 1,2}] {s.Name,-15} at {p}");
        }
        Console.WriteLine("    [ 0] Cancel");
        Console.Write("  > ");

        string? input = Console.ReadLine()?.Trim();
        if (!int.TryParse(input, out int num) || num < 1 || num > 20)
            return;

        var targetSlot = UibConstants.Jobs[num - 1];
        if (targetSlot == sourceSlot)
        {
            PrintError("Can't swap a job with itself.");
            return;
        }

        var oldSource = _uib.ReadPosition(sourceSlot);
        var oldTarget = _uib.ReadPosition(targetSlot);

        _uib.SwapPositions(sourceSlot, targetSlot);
        _hasUnsavedChanges = true;

        PrintSuccess($"Swapped {sourceSlot.Name} {oldSource} <-> {targetSlot.Name} {oldTarget}");
    }

    private void ManualPosition(JobSlot slot)
    {
        Console.Write($"  Enter X (0-{UibConstants.ScreenWidth}): ");
        string? xInput = Console.ReadLine()?.Trim();
        if (!int.TryParse(xInput, out int x))
        {
            PrintError("Invalid X value.");
            return;
        }

        Console.Write($"  Enter Y (0-{UibConstants.ScreenHeight}): ");
        string? yInput = Console.ReadLine()?.Trim();
        if (!int.TryParse(yInput, out int y))
        {
            PrintError("Invalid Y value.");
            return;
        }

        if (x < 0 || x > UibConstants.ScreenWidth || y < 0 || y > UibConstants.ScreenHeight)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"  Warning: ({x}, {y}) is outside the standard 1920x1080 area. Continue? [y/N]: ");
            Console.ResetColor();
            string? confirm = Console.ReadLine()?.Trim().ToLower();
            if (confirm != "y") return;
        }

        var oldPos = _uib.ReadPosition(slot);
        _uib.WritePosition(slot, new JobPosition(x, y));
        _hasUnsavedChanges = true;

        PrintSuccess($"{slot.Name} moved from {oldPos} to ({x}, {y}).");
    }

    private void ResetAll()
    {
        Console.Write("  Reset ALL jobs to default positions? [y/N]: ");
        string? confirm = Console.ReadLine()?.Trim().ToLower();
        if (confirm != "y") return;

        _uib.ResetAllPositionsToDefaults();
        _hasUnsavedChanges = true;
        PrintSuccess("All jobs reset to default positions.");
    }

    private void Save()
    {
        try
        {
            _uib.Save(outputPath);
            _hasUnsavedChanges = false;
            PrintSuccess($"Saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            PrintError($"Failed to save: {ex.Message}");
        }
    }

    private void ExportLayout()
    {
        Console.Write("  Layout name (or press Enter for default): ");
        string? name = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(name)) name = "Custom Layout";

        Console.Write("  Export path [layout.json]: ");
        string? path = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(path)) path = "layout.json";

        try
        {
            var config = LayoutConfig.Export(_uib, [], name);
            config.SaveToFile(path);
            PrintSuccess($"Layout exported to: {path}");
        }
        catch (Exception ex)
        {
            PrintError($"Failed to export: {ex.Message}");
        }
    }

    private void LoadLayout()
    {
        Console.Write("  JSON layout file path: ");
        string? path = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(path) || !File.Exists(path))
        {
            PrintError("File not found.");
            return;
        }

        try
        {
            var config = LayoutConfig.LoadFromFile(path);
            Console.WriteLine($"  Layout: \"{config.Name}\"");
            if (!string.IsNullOrEmpty(config.Description))
                Console.WriteLine($"  Description: {config.Description}");
            Console.WriteLine($"  Contains {config.Jobs.Count} job(s).");
            Console.Write("  Apply this layout? [y/N]: ");

            string? confirm = Console.ReadLine()?.Trim().ToLower();
            if (confirm != "y") return;

            var (applied, skipped) = config.Apply(_uib, []);
            _hasUnsavedChanges = true;

            PrintSuccess($"Applied {applied.Count} position(s).");
            if (skipped.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"  Skipped (unknown job names): {string.Join(", ", skipped)}");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            PrintError($"Failed to load layout: {ex.Message}");
        }
    }

    private bool ConfirmExit()
    {
        if (_hasUnsavedChanges)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("  You have unsaved changes. Save before exiting? [Y/n/cancel]: ");
            Console.ResetColor();
            string? input = Console.ReadLine()?.Trim().ToLower();
            if (input == "cancel" || input == "c") return false;
            if (input != "n") Save();
        }
        Console.WriteLine("  Goodbye!");
        return true;
    }

    // --- Display helpers ---

    private void PrintHeader()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("  ╔═══════════════════════════════════════════════════╗");
        Console.WriteLine("  ║   FFT Ivalice Chronicles - Job Tree Editor        ║");
        Console.WriteLine("  ╚═══════════════════════════════════════════════════╝");
        Console.ResetColor();
    }

    private void PrintJobTable()
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"  {"#",3}  {"Job",-15}  {"Current",14}  {"Default",14}  {"Modified",8}");
        Console.WriteLine($"  {"---",3}  {"---------------",-15}  {"--------------",14}  {"--------------",14}  {"--------",8}");
        Console.ResetColor();

        for (int i = 0; i < UibConstants.Jobs.Length; i++)
        {
            var slot = UibConstants.Jobs[i];
            var pos = _uib.ReadPosition(slot);
            bool modified = pos.X != slot.DefaultX || pos.Y != slot.DefaultY;

            if (modified) Console.ForegroundColor = ConsoleColor.Yellow;

            Console.Write($"  {i + 1,3}  {slot.Name,-15}");
            Console.Write($"  ({pos.X,5}, {pos.Y,4})");
            Console.Write($"  ({slot.DefaultX,5}, {slot.DefaultY,4})");
            Console.Write($"  {(modified ? "  *" : "")}");
            Console.WriteLine();

            Console.ResetColor();
        }
    }

    private void PrintMainMenu()
    {
        string unsaved = _hasUnsavedChanges ? " (unsaved changes!)" : "";
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"  Enter job # to edit  |  [S]ave{unsaved}  |  [R]eset all  |  [E]xport  |  [L]oad  |  [Q]uit");
        Console.ResetColor();
        Console.Write("  > ");
    }

    private static void PrintSuccess(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"  ✓ {msg}");
        Console.ResetColor();
    }

    private static void PrintError(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  ✗ {msg}");
        Console.ResetColor();
    }
}

