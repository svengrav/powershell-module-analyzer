using Microsoft.Extensions.Logging;
using ModuleAnalyzer.Core.Interfaces;
using ModuleAnalyzer.Core.Output;
using ModuleAnalyzer.Core.Visitor;

namespace ModuleAnalyzer.Core.Anaylzer;

public class ModuleAnalyzerBuilder
{
    private readonly string _modulePath;
    private readonly List<IAnalyzerOutputFormatter> _analyzerOutputs = new();
    private ModuleCommandVisitorFactory? _factory;
    private ILogger? _logger;

    private ModuleAnalyzerBuilder(string modulePath)
    {
        _modulePath = modulePath;
        _analyzerOutputs.Add(new ShortStatFormatter());
    }

    public static ModuleAnalyzerBuilder Create(string modulePath) => new ModuleAnalyzerBuilder(modulePath);

    public ModuleAnalyzerBuilder AddOutputFormatter(IAnalyzerOutputFormatter output)
    {
        _logger?.LogDebug("{object}: Output formatter {output} added", nameof(ModuleAnalyzerBuilder), nameof(output));
        _analyzerOutputs.Add(output);
        return this;
    }

    public ModuleAnalyzerBuilder AddLogger(ILogger? logger = null)
    {
        _logger = logger;
        return this;
    }

    public ModuleAnalyzerBuilder AddCommandVisitor<CommandVisitorFactoryClass>() where CommandVisitorFactoryClass : ModuleCommandVisitorFactory, new()
    {
        _logger?.LogDebug("{object}: CommandVisitorFactory {factory} added", nameof(ModuleAnalyzerBuilder), nameof(CommandVisitorFactoryClass));
        _factory = new CommandVisitorFactoryClass();
        return this;
    }

    public ModuleAnalyzer Build()
    {
        if (_logger is null)
            _logger = CreateDefaultLogger();

        if (_factory is null)
            _factory = new ModuleDefaultCommandVisitorFactory();

        var analyzer = new ModuleAnalyzer(_modulePath, _logger);
        analyzer.SetCommandVisitorFactory(_factory);

        _analyzerOutputs.ForEach(output => analyzer.AddOutputFormat(output));
        return analyzer;
    }

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
