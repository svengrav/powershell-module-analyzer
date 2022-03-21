using PsModuleAnalyzer.App.Factory;
using PsModuleAnalyzer.App.Output;
using PsModuleAnalyzer.Core.Anaylzer;
using PsModuleAnalyzer.DgmlFormatter;

namespace PsModuleAnalyzer.App
{
    public class Program
    {
        private static void Main(string[] args)
        {
            ModuleAnalyzer? analyzer = ModuleAnalyzerBuilder.Create("D:\\Repositories\\Go\\Source\\Go.psd1")
                .AddCommandVisitor<DefaultCommandVisitorFactory>()
                .AddDgmlFormatter("d:\\module.dgml")
                .AddOutputWriter(new MermaidOutput("d:\\module.md"))
                .Build();

            analyzer.Analyze();

        }
    }
}
