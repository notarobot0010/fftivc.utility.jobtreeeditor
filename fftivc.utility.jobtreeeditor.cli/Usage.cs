namespace fftivc.utility.jobtreeeditor.cli;

public static class Usage
{
    public static void PrintUsage()
    {
        Console.WriteLine(@"
  FFT Ivalice Chronicles - Job Tree Position Editor

  Usage:
    fftivc.utility.jobtreeeditor edit [input.uib] [-o output.uib]
        Interactive console editor. Opens the UIB, lets you swap or
        reposition jobs, then saves to the output path.

    fftivc.utility.jobtreeeditor apply <layout.json> [input.uib] [-o output.uib]
        Apply a JSON layout file to the UIB non-interactively.

    fftivc.utility.jobtreeeditor export [output.json] [input.uib]
        Export current UIB positions to a JSON layout file.

    fftivc.utility.jobtreeeditor export-defaults [output.json]
        Export the hardcoded default positions to JSON (no UIB needed).

  Defaults:
    Input:  ./ffto_job_tree.uib
    Output: ./output/ffto_job_tree.uib

  Examples:
    fftivc.utility.jobtreeeditor edit mymod/ffto_job_tree.uib -o mymod/output/ffto_job_tree.uib
    fftivc.utility.jobtreeeditor apply my_layout.json -o output/ffto_job_tree.uib
    fftivc.utility.jobtreeeditor export-defaults defaults.json
");
    }
}
