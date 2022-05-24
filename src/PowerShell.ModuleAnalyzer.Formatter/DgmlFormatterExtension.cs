using ModuleAnalyzer.Core.Anaylzer;

namespace ModuleAnalyzer.Formatter;

public static class DgmlFormatterExtension
{
    /// <summary>
    /// Formatter for the directed graph markup language.
    /// The formatter generates a module graph.
    /// </summary>
    /// <param name="destinationPath">Destination path for the result</param>
    public static ModuleAnalyzerBuilder AddDgmlFormatter(this ModuleAnalyzerBuilder moduleAnalyzerBuilder, string destinationPath)
    {
        moduleAnalyzerBuilder.AddOutputFormatter(new DgmlFormatter(destinationPath));
        return moduleAnalyzerBuilder;
    }
}
