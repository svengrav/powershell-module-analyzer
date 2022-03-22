using PsModuleAnalyzer.Core.Anaylzer;

namespace PsModuleAnalyzer.DgmlFormatter
{
    public static class DgmlFormatterExtension
    {
        public static ModuleAnalyzerBuilder AddDgmlFormatter(this ModuleAnalyzerBuilder moduleAnalyzerBuilder, string destinationPath)
        {
            moduleAnalyzerBuilder.AddOutputFormatter(new DgmlFormatter(destinationPath));
            return moduleAnalyzerBuilder;
        }
    }
}