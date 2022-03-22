using System.Linq;

namespace PsModuleAnalyzer.Core.Model
{
    public class ModuleCommand : IModuleCommand
    {
        private readonly string _commandFile;

        public string Name { get; private set; }
        public string Definition { get; private set; }
        public ModuleDefinition Module { get; private set; }
        public List<IModuleCommand> References { get => CalculateReferences(); }
        public List<IModuleCommand> ReferencedBy { get => CalculateReferencedBy(); }
        public List<ModuleCommandParameter> Parameters { get; private set; } = new();
        public int NumberOfReferencedBy { get; set; } = 0;
        public bool IsExternal { get; }
        public string Namespace { get; private set; }

        public decimal StabilityIndex { get => CalculateStabilityIndex(); }


        public ModuleCommand(string name, string definition, string commandFile, ModuleDefinition module)
        {
            Name = name;
            Definition = definition;
            IsExternal = false;
            Module = module;
            _commandFile = commandFile;

            ConfigureNamespace();
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

        private List<IModuleCommand> CalculateReferencedBy()
        {
            return Module.ModuleCommandCalls
                .Where(cmd => cmd.Target.Name == Name)
                .Select(cmd => cmd.Source)
                .ToList();
        }

        private List<IModuleCommand> CalculateReferences()
        {
            return Module.ModuleCommandCalls
                .Where(cmd => cmd.Source.Name == Name)
                .Select(cmd => cmd.Source)
                .ToList();
        }

        private decimal CalculateStabilityIndex()
        {
            return Math.Round((decimal) CalculateReferences().Count / (CalculateReferences().Count + CalculateReferencedBy().Count), 3);
        }

        private void ConfigureNamespace()
        {
            Namespace = _commandFile
                .Replace(Module.Path, "")
                .Replace($"{Name}.ps1", "")
                .Trim('\\')
                .Replace("\\", ".");
        }
    }
}
