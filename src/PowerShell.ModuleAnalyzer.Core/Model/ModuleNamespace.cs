namespace ModuleAnalyzer.Core.Model;

public class ModuleNamespace
{
    public string Id { get; set; }

    public ModuleNamespace(string @namespace)
    {
        Id = @namespace;
    }
}
