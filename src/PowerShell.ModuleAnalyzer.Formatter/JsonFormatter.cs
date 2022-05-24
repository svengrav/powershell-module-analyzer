using ModuleAnalyzer.Core.Interfaces;
using ModuleAnalyzer.Core.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ModuleAnalyzer.Formatter;

internal class JsonFormatter : IAnalyzerOutputFormatter
{
    private readonly string _destinationFile;

    public JsonFormatter(string destinationPath)
    {
        _destinationFile = destinationPath;
    }

    public void CreateAnalyzerOutupt(ModuleDefinition moduleDefinition)
    {
        var jsonModuleString = JsonConvert.SerializeObject(moduleDefinition, new JsonSerializerSettings() {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        });

        WriteToFile(jsonModuleString);
    }

    private void WriteToFile(string json) 
        => File.WriteAllText(_destinationFile, json);

}