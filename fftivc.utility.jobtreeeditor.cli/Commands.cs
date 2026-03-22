using fftivc.utility.jobtreeeditor.uib;

namespace fftivc.utility.jobtreeeditor.cli;

public class Commands
{
    /// <summary>
    /// Interactive editor mode.
    /// </summary>
    public static int RunEdit(string[] args)
    {
        string inputPath = ArgumentHelpers.FindInputPath(args);
        string outputPath = ArgumentHelpers.FindOutputPath(args);

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Error: File not found: {inputPath}");
            return 1;
        }

        try
        {
            var uib = new JobTreeUib(inputPath);
            var editor = new ConsoleEditor(uib, outputPath);
            editor.Run();
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return 1;
        }
    }

    /// <summary>
    /// Non-interactive mode: apply a JSON layout file.
    /// Usage: fftivc.utility.jobtreeeditor apply layout.json [input.uib] [-o output.uib]
    /// </summary>
    public static int RunApply(string[] args)
    {
        if (args.Length < 1)
        {
            Console.Error.WriteLine("Usage: fftivc.utility.jobtreeeditor apply <layout.json> [input.uib] [-o output.uib]");
            return 1;
        }

        string layoutPath = args[0];
        string inputPath = ArgumentHelpers.FindInputPath([.. args.Skip(1)]);
        string outputPath = ArgumentHelpers.FindOutputPath([.. args.Skip(1)]);

        if (!File.Exists(layoutPath))
        {
            Console.Error.WriteLine($"Error: Layout file not found: {layoutPath}");
            return 1;
        }
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Error: UIB file not found: {inputPath}");
            return 1;
        }

        try
        {
            var uib = new JobTreeUib(inputPath);
            var config = LayoutConfig.LoadFromFile(layoutPath);

            Console.WriteLine($"Applying layout \"{config.Name}\" to {inputPath}...");

            var (applied, skipped) = config.ApplyTo(uib);
            uib.Save(outputPath);

            Console.WriteLine($"Applied {applied.Count} positions. Saved to: {outputPath}");
            if (skipped.Count > 0)
                Console.WriteLine($"Skipped unknown jobs: {string.Join(", ", skipped)}");

            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return 1;
        }
    }

    /// <summary>
    /// Export the current UIB positions to a JSON file.
    /// </summary>
    public static int RunExport(string[] args)
    {
        string inputPath = ArgumentHelpers.FindInputPath(args);
        string jsonPath = "layout.json";

        // Check if first arg is a json output path
        if (args.Length > 0 && args[0].EndsWith(".json", StringComparison.OrdinalIgnoreCase))
        {
            jsonPath = args[0];
            args = [.. args.Skip(1)];
            inputPath = ArgumentHelpers.FindInputPath(args);
        }

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Error: UIB file not found: {inputPath}");
            return 1;
        }

        try
        {
            var uib = new JobTreeUib(inputPath);
            var config = LayoutConfig.FromUibFile(uib, "Exported from " + Path.GetFileName(inputPath));
            config.SaveToFile(jsonPath);
            Console.WriteLine($"Exported {config.Positions.Count} positions to: {jsonPath}");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return 1;
        }
    }

    /// <summary>
    /// Export the hardcoded default positions to a JSON file (no UIB needed).
    /// </summary>
    public static int RunExportDefaults(string[] args)
    {
        string jsonPath = args.Length > 0 ? args[0] : "defaults.json";

        try
        {
            var config = LayoutConfig.FromDefaults();
            config.SaveToFile(jsonPath);
            Console.WriteLine($"Default positions exported to: {jsonPath}");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return 1;
        }
    }
}
