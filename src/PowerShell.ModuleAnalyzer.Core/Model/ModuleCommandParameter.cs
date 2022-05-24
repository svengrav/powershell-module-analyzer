namespace ModuleAnalyzer.Core.Model;

public class ModuleCommandParameter
{
    public readonly IModuleCommand Command;
    public readonly string Name;
    public readonly string Type;

    public int References => CalculateReferences();

    public ModuleCommandParameter(IModuleCommand command, string name, string type)
    {
        Command = command;
        Name = name;
        Type = type;
    }

    /// <summary>
    /// Calculates the number of calls for this parameter.
    /// </summary>
    private int CalculateReferences() 
        => Command.ReferencedBy.Sum(commandCall => commandCall.Parameters.Count(param => param.Equals(this)));
}
