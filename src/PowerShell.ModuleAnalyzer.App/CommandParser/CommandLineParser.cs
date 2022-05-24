using CommandLine;

namespace ModuleAnalyzer.App.CommandParser;

internal static class CommandLineParser
{
    internal static void Execute(IEnumerable<string> args, Action<CommandLineOptions> action)
    {
        Parser.Default.ParseArguments<CommandLineOptions>(args)
            .WithParsed(action);
    }
}
