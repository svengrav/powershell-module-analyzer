using Microsoft.Extensions.Logging;
using ModuleAnalyzer.Core.Interfaces;
using ModuleAnalyzer.Core.Model;

namespace ModuleAnalyzer.Core.Output;

public class ShortStatFormatter : IAnalyzerOutputFormatter
{
    private readonly ILogger _logger;

    public ShortStatFormatter(ILogger? logger = null)
    {
        _logger = logger ?? CreateDefaultLogger();
    }

    public void CreateAnalyzerOutupt(ModuleDefinition moduleDefinition)
    {
        _logger.LogInformation("Short Stat -----------------------------------------");
        _logger.LogInformation(ToListItem("Name", moduleDefinition.Name));
        _logger.LogInformation(ToListItem("Path", moduleDefinition.Path));
        _logger.LogInformation(ToListItem("Total commands", moduleDefinition.ModuleCommands.Count));
        _logger.LogInformation(ToListItem("Total invoked commands", moduleDefinition.ModuleCommandCalls.Count));
        _logger.LogInformation(ToListItem("Total parameters", moduleDefinition.ModuleCommandParameters.Count));
        _logger.LogInformation(ToListItem("Total namespaces", moduleDefinition.ModuleNamespaces.Count));
        _logger.LogInformation(ToListItem("Total external commands", moduleDefinition.ModuleExternalCommand.Count));
        _logger.LogInformation(ToListItem("Line of code per command (median)", moduleDefinition.LineOfCodeMedian));
        _logger.LogInformation(ToListItem("Parameters per command (median)", moduleDefinition.CommandParameterMedian));
        _logger.LogInformation("----------------------------------------------------");
    }

    private static string ToListItem(string key, object value) => string.Format("{0,-35}: {1}", key, value);

    private static ILogger CreateDefaultLogger()
    {
        return LoggerFactory.Create(builder =>
                {
                    builder.AddSimpleConsole(options =>
                    {
                        options.SingleLine = true;
                        options.TimestampFormat = "hh:mm:ss ";
                        options.IncludeScopes = false;
                    })
                    .SetMinimumLevel(LogLevel.Debug);
                }).CreateLogger("");
    }
}
