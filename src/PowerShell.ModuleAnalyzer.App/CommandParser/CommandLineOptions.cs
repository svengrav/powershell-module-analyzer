using CommandLine;

namespace ModuleAnalyzer.App.CommandParser;

public class CommandLineOptions
{
    [Option('d', "debug", Required = false, HelpText = "Set output to verbose messages.")]
    public bool Verbose { get; set; }

    [Option('p', "path", Required = true, HelpText = "Set module path.")]
    public string Path { get; set; }

    [Option('o', "outputdir", Required = true, HelpText = "Directory for the result.")]
    public string OutputDir { get; set; }

    [Option('n', "outputname", Required = false, HelpText = "Directory for the result.")]
    public string OutputName { get; set; } = "result";
}