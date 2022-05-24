using ModuleAnalyzer.Core.Anaylzer;

namespace ModuleAnalyzer.Formatter;

public static class CsvFormatterExtension
{
    public static ModuleAnalyzerBuilder AddCSVFormatter(this ModuleAnalyzerBuilder moduleAnalyzerBuilder, string destinationPath, bool compact = false)
    {
        if (compact)
            moduleAnalyzerBuilder.AddOutputFormatter(new CsvCompactFormatter(destinationPath));
        else
            moduleAnalyzerBuilder.AddOutputFormatter(new CsvFormatter(destinationPath));

        return moduleAnalyzerBuilder;
    }
}
