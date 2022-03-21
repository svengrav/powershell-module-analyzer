namespace PsModuleAnalyzer.Core.Model
{
    public class ModuleCommandParameter
    {
        public readonly ModuleCommand Command;
        public readonly string Name;
        public readonly string Type;

        public ModuleCommandParameter(ModuleCommand command, string name, string type)
        {
            Command = command;
            Name = name;
            Type = type;
        }
    }
}
