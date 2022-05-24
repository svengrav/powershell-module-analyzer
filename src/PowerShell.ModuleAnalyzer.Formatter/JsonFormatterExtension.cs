using ModuleAnalyzer.Core.Anaylzer;

namespace ModuleAnalyzer.Formatter;

public static class JsonFormatterExtension {

    /// <summary>
    /// Formatter for json text.
    /// </summary>
    /// <param name="destinationPath">Destination path for the result</param>
    public static ModuleAnalyzerBuilder AddJsonFormatter(this ModuleAnalyzerBuilder moduleAnalyzerBuilder, string destinationPath)
    {
        moduleAnalyzerBuilder.AddOutputFormatter(new JsonFormatter(destinationPath));
        return moduleAnalyzerBuilder;
    }
}
