using ModuleAnalyzer.Core.Model;

namespace ModuleAnalyzer.Core.Interfaces;

/// <summary>
/// Interface to implement custom outputs for the analyzer.
/// </summary>
public interface IAnalyzerOutputFormatter
{
    public void CreateAnalyzerOutupt(ModuleDefinition moduleDefinition);
}
