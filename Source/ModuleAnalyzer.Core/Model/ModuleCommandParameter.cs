namespace PsModuleAnalyzer.Core.Model
{
    public class ModuleCommandParameter
    {
        public readonly ModuleCommand Command;
        public readonly string Name;
        public readonly string Type;
        public int References { get => CalculateReferences(); }

        public ModuleCommandParameter(ModuleCommand command, string name, string type)
        {
            Command = command;
            Name = name;
            Type = type;
        }

        private int CalculateReferences()
        {
            return Command.Module.ModuleCommandCalls
                .Where(call => call.Target.Name == Command.Name)
                .Sum(call => call.Parameters.Count(param => param.Name == Name));
        }
    }
}
