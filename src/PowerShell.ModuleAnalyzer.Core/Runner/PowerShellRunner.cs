using Microsoft.Extensions.Logging;
using System.Management.Automation;

namespace ModuleAnalyzer.Core.Runner;

internal class PowerShellRunner
{
    public static PowerShell CreatePowerShell(ILogger? logger = null)
    {
        var powerShell = PowerShell.Create();

        powerShell.Streams.Error.DataAdded += (sender, e)
            => LogErrror<ErrorRecord>(sender, e, logger);

        powerShell.Streams.Information.DataAdded += (sender, e)
           => LogProgress<InformationRecord>(sender, e, logger);

        powerShell.Streams.Progress.DataAdded += (sender, e)
            => LogProgress<ProgressRecord>(sender, e, logger);

        powerShell.Streams.Warning.DataAdded += (sender, e)
            => LogProgress<WarningRecord>(sender, e, logger);

        return powerShell;
    }

    private static void LogProgress<T>(object? sender, DataAddedEventArgs e, ILogger? logger)
    {
        if (sender is PSDataCollection<T> psDataCollection)
            logger?.LogTrace("PowerShell Stream: {data}", psDataCollection[e.Index]?.ToString());
    }

    private static void LogErrror<T>(object? sender, DataAddedEventArgs e, ILogger? logger)
    {
        if (sender is PSDataCollection<T> psDataCollection)
            logger?.LogCritical("PowerShell Stream: {data}", psDataCollection[e.Index]?.ToString());
    }
}
