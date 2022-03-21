using ModuleAnalyzer.Core.Model;

namespace ModuleAnalyzer.Core.Interfaces
{
    public interface IAnalyzerOutput
    {
        public void CreateAnalyzerOutupt(List<ModuleCommandCall> commandCalls);
    }
}
