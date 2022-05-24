namespace ModuleAnalyzer.Core.Model;

public class ModuleExternalCommand : IModuleCommand
{
    public string Name { get; private set; }
    public int TotalReferencedBy { get; set; } = 0;
    public bool IsExternal { get; }
    public string _commandFile { get; private set; } = "External";

    public ModuleNamespace Namespace => new("");

    public ICollection<ModuleCommandParameter> Parameters { get; } = new HashSet<ModuleCommandParameter>();

    public ICollection<ModuleCommandCall> References { get; } = new HashSet<ModuleCommandCall>();

    public ICollection<ModuleCommandCall> ReferencedBy { get; } = new HashSet<ModuleCommandCall>();

    public ModuleExternalCommand(string name)
    {
        Name = name;
        IsExternal = true;
    }

    public Dictionary<string, string> GetParametersAsDictionary()
    {
        var parameters = new Dictionary<string, string>();
        foreach (var parameter in Parameters)
        {
            parameters.Add(parameter.Name, parameter.Type);
        }
        return parameters;
    }

    IDictionary<string, string> IModuleCommand.GetParametersAsDictionary() => throw new NotImplementedException();
}
