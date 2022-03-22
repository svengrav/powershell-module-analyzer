using System.Reflection;

namespace PsModuleAnalyzer.Core.Model
{
    public class ModuleCommandCall
    {
        private readonly ModuleDefinition _module;
        public readonly IModuleCommand Source;
        public readonly IModuleCommand Target;
        public readonly List<ModuleCommandParameter> Parameters = new();
        public int References { get => CalculateReferences(); }

        public ModuleCommandCall(IModuleCommand source, IModuleCommand target, ModuleDefinition module)
        {
            _module = module;
            Source = source;
            Target = target;
        }

        private int CalculateReferences() 
            => _module.ModuleCommandCalls
                .Count(calls => calls.Source.Name == Source.Name && calls.Target.Name == Target.Name);
    }
}
