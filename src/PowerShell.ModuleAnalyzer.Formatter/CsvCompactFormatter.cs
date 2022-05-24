using ModuleAnalyzer.Core.Interfaces;
using ModuleAnalyzer.Core.Model;

namespace ModuleAnalyzer.Formatter;

public class CsvCompactFormatter : IAnalyzerOutputFormatter
{
    private readonly string _destinationFile;

    public CsvCompactFormatter(string destinationFile)
    {
        _destinationFile = destinationFile;
    }

    public void CreateAnalyzerOutupt(ModuleDefinition moduleDefinition) 
        => WriteToFile(_destinationFile, CreateCsvContent(moduleDefinition.ModuleCommands));

    private static List<string> CreateCsvContent(HashSet<ModuleCommand> moduleCommands)
    {
        var csvContent = new List<string>()
        {
            CreateCsvContentHeader()
        };

        moduleCommands.ToList().ForEach(command =>
        {
            csvContent.Add(CreateCsvContentLine(command));
        });

        return csvContent;
    }

    private static string CreateCsvContentHeader()
        => string.Join(";", new List<string>
        {
            "Name",
            "Namespace",
            "Invokes (Module)",
            "Invokes (External)",
            "InvokedBy",
            "References (Unique)",
            "ReferencedBy (Unique)",
            "StabilityIndex",
            "LinesOfCode",
            "Parameters"
        });

    private static string CreateCsvContentLine(ModuleCommand moduleCommand)
        =>  string.Join(";", new List<string>
        {
            moduleCommand.Name,
            moduleCommand.Namespace?.Id,
            moduleCommand.References.Where(cmd => !cmd.Target.IsExternal).ToList().Count.ToString(),
            moduleCommand.References.Where(cmd => cmd.Target.IsExternal).ToList().Count.ToString(),
            moduleCommand.ReferencedBy.Count.ToString(),
            moduleCommand.TotalUniqueReferenced.ToString(),
            moduleCommand.TotalUniqueReferencedBy.ToString(),
            moduleCommand.StabilityIndex.ToString(),
            moduleCommand.LinesOfCode.ToString(),
            CreateCsvParameterColumn(moduleCommand.Parameters)
        });

    private static string CreateCsvParameterColumn(ICollection<ModuleCommandParameter> moduleCommandParameters)
    {
        var paramlist = new List<string>();
        foreach(var param in moduleCommandParameters)
            paramlist.Add($"{param.Name} [{param.Type}]({param.References})");

        return string.Join(" / ", paramlist);
    }

    private static void WriteToFile(string destinationFile, List<string> contentLines) 
        => File.WriteAllLines(destinationFile, contentLines);

}
