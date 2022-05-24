namespace ModuleAnalyzer.Core.Model;

public class ModuleCommand : IModuleCommand
{
    public readonly string CommandFile;

    public string Name { get; private set; }
    internal string Definition { get; set; }

    public int TotalReferencedBy => ReferencedBy.Count;
    public bool IsExternal { get; }
    public ModuleNamespace Namespace { get; set; }

    public ICollection<ModuleCommandParameter> Parameters { get; } = new HashSet<ModuleCommandParameter>();
    public ICollection<ModuleCommandCall> References { get; } = new HashSet<ModuleCommandCall>();
    public ICollection<ModuleCommandCall> ReferencedBy { get; } = new HashSet<ModuleCommandCall>();

    public int TotalUniqueReferencedBy => ReferencedBy.GroupBy(cmd => cmd.Target).Count();
    public int TotalUniqueReferenced => ReferencedBy.GroupBy(cmd => cmd.Source).Count();

    public decimal StabilityIndex => CalculateStabilityIndex();
    public int LinesOfCode = 0;

    public ModuleCommand(string name, string definition, string commandFile)
    {
        Name = name;
        Definition = definition;
        IsExternal = false;
        CommandFile = commandFile;
    }

    public IDictionary<string, string> GetParametersAsDictionary()
    {
        var parameters = new Dictionary<string, string>();
        foreach (var parameter in Parameters)
        {
            parameters.Add(parameter.Name, parameter.Type);
        }
        return parameters;
    }

    private decimal CalculateStabilityIndex()
    {
        if (References.Count + ReferencedBy.Count == 0)
            return 0;

        return Math.Round((decimal)References.Count / (References.Count + ReferencedBy.Count), 3);
    }
}
