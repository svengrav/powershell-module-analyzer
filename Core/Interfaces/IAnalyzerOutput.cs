using PSModuleAnalyzer.Core.Model;

namespace PSModuleAnalyzer.Core.Interfaces
{
    public interface IAnalyzerOutput
    {
        public void CreateAnalyzerOutupt(List<ModuleCommandCall> commandCalls);
    }
}
