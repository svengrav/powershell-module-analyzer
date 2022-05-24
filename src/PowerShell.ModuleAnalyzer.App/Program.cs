using ModuleAnalyzer.App.CommandParser;
using ModuleAnalyzer.Core.Anaylzer;
using ModuleAnalyzer.Core.Visitor;
using ModuleAnalyzer.Formatter;

namespace ModuleAnalyzer.App;

public class Program
{
    private static void Main(string[] args)
    {
        CommandLineParser.Execute(args, options =>
        {
            CreateOutputDirectory(options.OutputDir);

            var outputFile = Path.Combine(options.OutputDir, options.OutputName);
            var analyzer = ModuleAnalyzerBuilder.Create(options.Path)
                .AddCommandVisitor<ModuleDefaultCommandVisitorFactory>()
                .AddDgmlFormatter(CreateOutputFilePath(options.OutputDir, options.OutputName, "dgml"))
                .AddCSVFormatter(CreateOutputFilePath(options.OutputDir, $"{options.OutputName}-invokes", "csv"))
                .AddCSVFormatter(CreateOutputFilePath(options.OutputDir, $"{options.OutputName}-compact", "csv"), true)
                //.AddJsonFormatter(CreateOutputFilePath(options.OutputDir, options.OutputName, "json"))
                .AddLogger()
                .Build();

            analyzer.Analyze();
        });
    }

    private static string CreateOutputFilePath(string outputDirectoryPath, string outputName, string outputExtension)
        => Path.Combine(outputDirectoryPath, $"{outputName}-{DateTime.Now:ddMMyyyy}.{outputExtension.TrimStart('.')}");

    private static void CreateOutputDirectory(string outputDirectoryPath)
    {
        if (!Directory.Exists(outputDirectoryPath))
            Directory.CreateDirectory(outputDirectoryPath);
    }
}