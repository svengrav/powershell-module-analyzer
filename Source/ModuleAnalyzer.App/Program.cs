using ModuleAnalyzer.App.Implementation.Factory;
using ModuleAnalyzer.Core.Anaylzer;
using ModuleAnalyzer.DgmlFormatter;
using PSModuleAnalyzer.Implementation.Output;

namespace ModuleAnalyzer.App
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
