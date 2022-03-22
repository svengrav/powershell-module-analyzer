using PsModuleAnalyzer.Core.Model;

namespace PsModuleAnalyzer.Core.Interfaces
{
    public interface IAnalyzerOutput
    {
        public void CreateAnalyzerOutupt(ModuleDefinition moduleDefinition);
    }
}
