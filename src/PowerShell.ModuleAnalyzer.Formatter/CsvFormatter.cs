using ModuleAnalyzer.Core.Interfaces;
using ModuleAnalyzer.Core.Model;

namespace ModuleAnalyzer.Formatter;

public class CsvFormatter : IAnalyzerOutputFormatter
{
    private readonly string _destinationFile;

    public CsvFormatter(string destinationFile)
    {
        _destinationFile = destinationFile;
    }

    public void CreateAnalyzerOutupt(ModuleDefinition moduleDefinition) 
        => WriteToFile(CreateCsvContent(moduleDefinition.ModuleCommandCalls));

    private static List<string> CreateCsvContent(ICollection<ModuleCommandCall> moduleCommandCalls)
    {
        var csvContent = new List<string>() {
            CreateCsvContentHeader()
        };

        moduleCommandCalls.Where(s => !s.Target.IsExternal).ToList().ForEach(command =>
        {
            csvContent.Add(CreateCsvContentLine(command));
        });

        return csvContent;
    }

    private static string CreateCsvContentLine(ModuleCommandCall commandCall)
    {
        var columns = new List<string>
        {
            commandCall.Source.Name,
            commandCall.Source.Namespace.Id,
            commandCall.Target.Name
        };

        var parameters = commandCall.Parameters.Select(param => param.Name);
        columns.AddRange(parameters);

        return string.Join(";", columns);
    }

    private static string CreateCsvContentHeader()
        => string.Join(";", new List<string>
        {
            "Name",
            "Namespace",
            "Target",
            "Parameters"
        });

    private void WriteToFile(List<string> contentLines) => File.WriteAllLines(_destinationFile, contentLines);
}
