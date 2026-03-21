using fftivc.utility.jobtreeeditor.cli;

if (args.Length == 0 || args.Contains("--help") || args.Contains("-h"))
{
    Usage.PrintUsage();
    return args.Contains("--help") || args.Contains("-h") ? 0 : 1;
}

string command = args[0].ToLower();

return command switch
{
    "edit" => Commands.RunEdit([.. args.Skip(1)]),
    "apply" => Commands.RunApply([.. args.Skip(1)]),
    "export" => Commands.RunExport([.. args.Skip(1)]),
    "export-defaults" => Commands.RunExportDefaults([.. args.Skip(1)]),
    _ => Commands.RunEdit(args), // If no command, treat all args as the edit command
};