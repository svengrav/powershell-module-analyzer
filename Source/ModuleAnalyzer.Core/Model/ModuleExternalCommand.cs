namespace PsModuleAnalyzer.Core.Model
{
    public class ModuleExternalCommand : IModuleCommand
    {
        public string Name { get; private set; }
        public List<IModuleCommand> ReferencedBy { get; }
        public List<IModuleCommand> References { get => new(); }
        public List<ModuleCommandParameter> Parameters { get => new(); }
        public int NumberOfReferencedBy { get; set; } = 0;
        public bool IsExternal { get; }
        public string _commandFile { get; private set; } = "External";

        public string Namespace { get => ""; }


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
