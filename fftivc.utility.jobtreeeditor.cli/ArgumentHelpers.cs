namespace fftivc.utility.jobtreeeditor.cli;

public class ArgumentHelpers
{
    public static string FindInputPath(string[] args)
    {
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i] == "-i" || args[i] == "--input")
                return args[i + 1];
        }

        foreach (var arg in args)
        {
            if (!arg.StartsWith("-") && arg.EndsWith(".uib", StringComparison.OrdinalIgnoreCase))
                return arg;
        }

        return Constants.DefaultFileName;
    }

    public static string FindOutputPath(string[] args)
    {
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i] == "-o" || args[i] == "--output")
                return args[i + 1];
        }

        return Path.Combine(Constants.DefaultOutputDir, Constants.DefaultFileName);
    }
}
