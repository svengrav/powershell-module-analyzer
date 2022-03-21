namespace PSModuleAnalyzer.Core.Model
{
    public class ModuleCommandCall
    {
        public readonly IModuleCommand Target;
        public readonly IModuleCommand Source;
        public readonly List<ModuleCommandParameter> Parameters = new();

        public ModuleCommandCall(IModuleCommand source, IModuleCommand target)
        {
            Source = source;
            Target = target;
        }
    }
}
