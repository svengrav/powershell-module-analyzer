using PSModuleAnalyzer.Core.Model;

namespace PSModuleAnalyzer.Core.Repository
{
    public class ModuleDefinition
    {
        public readonly string Name;
        public readonly string Path;
        public readonly HashSet<ModuleCommand> ModuleCommands = new();
        public readonly List<ModuleCommandCall> ModuleCommandCalls = new();
        public readonly List<ModuleCommandParameter> ModuleCommandParameters = new();
        public readonly List<ModuleExternalCommand> ModuleExternalCommand = new();

        public ModuleDefinition(string name, string path)
        {
            Name = name;
            Path = path;
        }

        public ModuleCommand? GetModuleCommand(string name)
        {
            return ModuleCommands.FirstOrDefault(command => command.Name == name);
        }

        public ModuleExternalCommand? GetModuleExternalCommand(string name)
        {
            return ModuleExternalCommand.FirstOrDefault(command => command.Name == name);
        }

        public HashSet<ModuleCommand> AddModuleCommands(HashSet<ModuleCommand> moduleCommands)
        {
            foreach(var command in moduleCommands)
            {
                ModuleCommands.Add(command);
            }
            return ModuleCommands;
        }

        public ModuleExternalCommand AddExternalModuleCommand(string name)
        {
            ModuleExternalCommand? moduleCommand = GetModuleExternalCommand(name);
            if (moduleCommand == null)
            {
                moduleCommand = new ModuleExternalCommand(name);
                ModuleExternalCommand.Add(moduleCommand);
            }

            return moduleCommand;
        }

        public ModuleCommandCall AddModuleCommandCall(ModuleCommandCall moduleCommand)
        {
            ModuleCommandCalls.Add(moduleCommand);

            RefreshTargetCommandReferences(moduleCommand.Target);
            return moduleCommand;
        }

        public bool IsModuleCommand(string name)
        {
            return ModuleCommands.FirstOrDefault(command => command.Name == name) != null ? true : false;
        }

        public ModuleExternalCommand AddModuleExternalCommandCall(string name, IModuleCommand calledBy)
        {
            ModuleExternalCommand? moduleCommand = GetModuleExternalCommand(name);
            if (moduleCommand == null)
            {
                moduleCommand = new ModuleExternalCommand(name);
                ModuleExternalCommand.Add(moduleCommand);
            }

            ModuleCommandCalls.Add(new ModuleCommandCall(calledBy, moduleCommand));
            return moduleCommand;
        }

        private void RefreshTargetCommandReferences(IModuleCommand moduleCommand)
        {
            moduleCommand.NumberOfReferencedBy = ModuleCommandCalls.Count(c => c.Target.Name == moduleCommand.Name);
        }

    }
}
