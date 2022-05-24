namespace ModuleAnalyzer.Core.Model;

public class ModuleCommandCall
{
    public readonly IModuleCommand Source;
    public readonly IModuleCommand Target;
    public readonly HashSet<ModuleCommandParameter> Parameters = new();

    public ModuleCommandCall(IModuleCommand source, IModuleCommand target)
    {
        Source = source;
        Target = target;

        source.References.Add(this);
        target.ReferencedBy.Add(this);
    }
}
