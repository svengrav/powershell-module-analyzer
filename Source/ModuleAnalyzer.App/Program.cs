using PsModuleAnalyzer.App.Factory;
using PsModuleAnalyzer.App.Output;
using PsModuleAnalyzer.Core.Anaylzer;
using PsModuleAnalyzer.DgmlFormatter;
using PsModuleAnaylzer.CSV;

namespace PsModuleAnalyzer.App
{
    public class Program
    {
        private static void Main(string[] args)
        {
            ModuleAnalyzer? analyzer = ModuleAnalyzerBuilder.Create("D:\\Repositories\\Go\\Source\\Go.psd1")
                .AddCommandVisitor<DefaultCommandVisitorFactory>()
                .AddDgmlFormatter("d:\\module.dgml")
                .AddCSVFormatter("d:\\module.csv")
                .AddCSVFormatter("d:\\module-compact.csv", true)
                .AddOutputFormatter(new MermaidOutput("d:\\module.md"))
                .Build();

            analyzer.Analyze();

        }
    }
}
