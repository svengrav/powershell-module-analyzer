using ModuleAnalyzer.Core.Anaylzer;

namespace ModuleAnalyzer.DgmlFormatter
{
    public static class DgmlFormatterExtension
    {
        public static ModuleAnalyzerBuilder AddDgmlFormatter(this ModuleAnalyzerBuilder moduleAnalyzerBuilder, string destinationPath)
        {
            moduleAnalyzerBuilder.AddOutputWriter(new DgmlFormatter(destinationPath));
            return moduleAnalyzerBuilder;
        }
    }
}