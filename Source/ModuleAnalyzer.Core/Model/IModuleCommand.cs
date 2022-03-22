namespace PsModuleAnalyzer.Core.Model
{
    public interface IModuleCommand
    {
        string Name { get; }
        string Namespace { get; }
        int NumberOfReferencedBy { get; set; }
        List<ModuleCommandParameter> Parameters { get; }
        List<IModuleCommand> References { get; }
        List<IModuleCommand> ReferencedBy { get; }
        bool IsExternal { get; }

        Dictionary<string, string> GetParametersAsDictionary();
    }
}