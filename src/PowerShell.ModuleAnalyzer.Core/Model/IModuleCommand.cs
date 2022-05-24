namespace ModuleAnalyzer.Core.Model;

public interface IModuleCommand
{
    /// <summary>
    /// Name of the command.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Tags the command as external. 
    /// An external command is a function which is called by the module and
    /// not defined as part of the module.
    /// </summary>
    bool IsExternal { get; }

    /// <summary>
    /// Returns how often this command is called by other commands.
    /// </summary>
    int TotalReferencedBy { get; }

    /// <summary>
    /// The namespace is the local path of the source file inside the module.
    /// </summary>
    ModuleNamespace Namespace { get; }

    /// <summary>
    /// Contains all command parameters.
    /// </summary>
    ICollection<ModuleCommandParameter> Parameters { get; }

    /// <summary>
    /// Contains all the command calls of this command.
    /// </summary>
    ICollection<ModuleCommandCall> References { get; }

    /// <summary>
    /// Contains all the commands that refer to this command.
    /// </summary>
    ICollection<ModuleCommandCall> ReferencedBy { get; }

    /// <summary>
    /// Returns the parameters as dictionary.
    /// </summary>
    IDictionary<string, string> GetParametersAsDictionary();
}
