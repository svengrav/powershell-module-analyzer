namespace ModuleAnalyzer.Core.Model
{
    public class ModuleExternalCommand : IModuleCommand
    {
        public string Name { get; private set; }
        public List<ModuleCommand> ReferencedBy { get; private set; } = new();
        public List<ModuleCommandParameter> Parameters { get; private set; } = new();
        public int NumberOfReferencedBy { get; set; } = 0;
        public bool IsExternal { get; }
        public string Namespace { get; private set; } = "External";

        public ModuleExternalCommand(string name)
        {
            Name = name;
            IsExternal = true;
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
