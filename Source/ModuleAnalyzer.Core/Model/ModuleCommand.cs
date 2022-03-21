namespace PsModuleAnalyzer.Core.Model
{
    public class ModuleCommand : IModuleCommand
    {
        public string Name { get; private set; }
        public string Definition { get; private set; }
        public List<ModuleCommand> References { get; private set; } = new();
        public List<ModuleCommand> ReferencedBy { get; private set; } = new();
        public List<ModuleCommandParameter> Parameters { get; private set; } = new();
        public int NumberOfReferencedBy { get; set; } = 0;
        public bool IsExternal { get; }
        public string Namespace { get; private set; }

        public ModuleCommand(string name, string definition, string @namespace)
        {
            Name = name;
            Definition = definition;
            IsExternal = false;
            Namespace = @namespace;
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
    }
}
